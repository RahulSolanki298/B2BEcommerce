using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Helpers;
using API.ViewModals;
using Core.Entities;
using Core.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierRepository _supplierRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public SuppliersController(ISupplierRepository supplierRepo,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            RoleManager<IdentityRole> roleManager)
        {
            _supplierRepo = supplierRepo;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }


        [HttpGet("SupplierList")]
        public async Task<ActionResult> GetApplicationUserList()
        {
            var data= await _supplierRepo.GetSuppierListAsync();
            return Ok(data);
        }

        [HttpGet("Supplier/{supplirId}")]
        public async Task<ActionResult> GetApplicationUser(string supplirId)
        {
            var data = await _supplierRepo.GetSupplierByIdAsync(supplirId);
            return Ok(data);
        }

        [HttpPost("create-supplier")]
        public async Task<ActionResult> CreateSuppliersUser([FromBody] CreateAdminUserModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Username and password are required.");
            }


            // Check if the admin role exists, if not create it
            var roleExist = await _roleManager.RoleExistsAsync("Suppliers");
            if (!roleExist)
            {
                var role = new IdentityRole("Suppliers");
                await _roleManager.CreateAsync(role);
            }

            // Create a new user
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                NormalizedUserName = model.UserName.ToUpper(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.UserName,
                TextPassword = model.Password,
                AadharCardNo = model.AadharCardNo,
                PancardNo = model.PancardNo,
                Gender = model.Gender,
                PhoneNumber = model.PhoneNumber,
                MiddleName = model.MiddleName,
                NormalizedEmail = model.UserName.ToUpper(),
                ActivationStatus = SD.Activated
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // Assign the 'Admin' role to the new user
            await _userManager.AddToRoleAsync(user, "Suppliers");

            // Generate a JWT token for the newly created admin user
            var token = await GenerateJwtToken(user);

            return Ok(new
            {
                Success = true,
                Message = "Supplier user created successfully.",
                Token = token
            });
        }

        [HttpDelete("delete-supplier/{userId}")]
        public async Task<ActionResult> DeleteSupplierUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("UserId is required.");
            }

            // Find the user by username
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Supplier user not found.");
            }

            // Remove the user from the 'Suppliers' role
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Suppliers"))
            {
                await _userManager.RemoveFromRoleAsync(user, "Suppliers");
            }
            else
            {
                return NotFound(new
                {
                    Success=false,
                    Message="Supplier is not found."
                });
            }

            // Delete the user
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new
            {
                Success = true,
                Message = "Supplier user deleted successfully."
            });
        }


        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, "Suppliers")
            };

            // Retrieve the secret key, issuer, and audience from appsettings
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Token:Issuer"], // Use "Token:Issuer"
                audience: _configuration["Token:Audience"], // Use "Token:Audience"
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
