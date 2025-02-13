using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.ViewModals;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }

        // Admin Creation API
        [HttpPost("create-admin")]
        public async Task<ActionResult> CreateAdminUser([FromBody] CreateAdminUserModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Username and password are required.");
            }

            // Check if the admin role exists, if not create it
            var roleExist = await _roleManager.RoleExistsAsync("Admin");
            if (!roleExist)
            {
                var role = new IdentityRole("Admin");
                await _roleManager.CreateAsync(role);
            }

            // Create a new user
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.UserName,
                TextPassword = model.Password
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // Assign the 'Admin' role to the new user
            await _userManager.AddToRoleAsync(user, "Admin");

            // Generate a JWT token for the newly created admin user
            var token = await GenerateJwtToken(user);

            return Ok(new
            {
                Success = true,
                Message = "Admin user created successfully.",
                Token = token
            });
        }


        [HttpPost("admin-login")]
        public async Task<ActionResult> AdminLogin([FromBody] AdminLoginModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Username and password are required.");
            }

            // Find the user by username (email)
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            // Check if the user is an admin by checking their role
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            if (!isAdmin)
            {
                return Unauthorized("You are not authorized to login as an admin.");
            }

            // Validate the password
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (!result.Succeeded)
            {
                return Unauthorized("Invalid username or password.");
            }

            // Generate the JWT token
            var token = await GenerateJwtToken(user);

            return Ok(new
            {
                Success = true,
                Message = "Admin logged in successfully.",
                Token = token
            });
        }

        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            // Sign the user out
            await _signInManager.SignOutAsync();

            return Ok(new
            {
                Success = true,
                Message = "User logged out successfully."
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
        new Claim(ClaimTypes.Role, "Admin")
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
