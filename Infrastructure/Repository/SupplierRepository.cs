using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class SupplierRepository
    {
        private readonly ApplicationDBContext _context;
        public SupplierRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<ApplicationUser>> GetSuppierListAsync()
        {

            return await (from sup in _context.ApplicationUser
                        join urole in _context.UserRoles on sup.Id equals urole.UserId
                        join role in _context.Roles on urole.RoleId equals role.Id
                        where role.Name == "Suppliers"
                        select new ApplicationUser
                        {
                            Id = sup.Id,
                            Email = sup.Email,
                            PhoneNumber = sup.PhoneNumber,
                            UserName = sup.UserName,
                            PancardNo = sup.PancardNo,
                            FirstName = sup.FirstName,
                            LastName = sup.LastName,
                            AadharCardNo = sup.AadharCardNo,
                            Gender = sup.Gender,
                            TextPassword = sup.TextPassword,
                            NormalizedUserName = sup.NormalizedUserName,
                            MiddleName = sup.MiddleName,
                            ActivationStatus = sup.ActivationStatus,
                            TwoFactorEnabled = sup.TwoFactorEnabled,

                        }).ToListAsync();

        }
    }
}
