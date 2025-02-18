using Core.Entities;
using Core.IRepository;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class BuyerRepository : IBuyerRepository
    {
        private readonly ApplicationDBContext _context;

        public BuyerRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        
        public async Task<IReadOnlyList<ApplicationUser>> GetBuyerListAsync() 
        {
            return await (from sup in _context.ApplicationUser
                          join urole in _context.UserRoles on sup.Id equals urole.UserId
                          join role in _context.Roles on urole.RoleId equals role.Id
                          where role.Name == "Buyers"
                          select new ApplicationUser
                          {
                              Id = sup.Id,
                              Email = sup.Email,
                              NormalizedEmail = sup.NormalizedEmail,
                              PhoneNumber = sup.PhoneNumber,
                              UserName = sup.UserName,
                              PancardNo = sup.PancardNo,
                              FirstName = sup.FirstName,
                              MiddleName = sup.MiddleName,
                              LastName = sup.LastName,
                              AadharCardNo = sup.AadharCardNo,
                              Gender = sup.Gender,
                              TextPassword = sup.TextPassword,
                              NormalizedUserName = sup.NormalizedUserName,
                              ActivationStatus = sup.ActivationStatus,
                              TwoFactorEnabled = sup.TwoFactorEnabled,
                              AccessFailedCount = sup.AccessFailedCount,

                          }).ToListAsync();

        }

        public async Task<ApplicationUser> GetBuyerByIdAsync(string userId) 
        {
            return await (from sup in _context.ApplicationUser
                          join urole in _context.UserRoles on sup.Id equals urole.UserId
                          join role in _context.Roles on urole.RoleId equals role.Id
                          where role.Name == "Buyers" && sup.Id == userId
                          select new ApplicationUser
                          {
                              Id = sup.Id,
                              Email = sup.Email,
                              NormalizedEmail = sup.NormalizedEmail,
                              PhoneNumber = sup.PhoneNumber,
                              UserName = sup.UserName,
                              PancardNo = sup.PancardNo,
                              FirstName = sup.FirstName,
                              MiddleName = sup.MiddleName,
                              LastName = sup.LastName,
                              AadharCardNo = sup.AadharCardNo,
                              Gender = sup.Gender,
                              TextPassword = sup.TextPassword,
                              NormalizedUserName = sup.NormalizedUserName,
                              ActivationStatus = sup.ActivationStatus,
                              TwoFactorEnabled = sup.TwoFactorEnabled,
                              AccessFailedCount = sup.AccessFailedCount,
                          }).FirstOrDefaultAsync();

        }

        
    }
}
