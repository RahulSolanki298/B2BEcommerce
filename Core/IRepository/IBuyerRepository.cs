using Core.Entities;

namespace Core.IRepository
{
    public interface IBuyerRepository
    {
        Task<IReadOnlyList<ApplicationUser>> GetBuyerListAsync();

        Task<ApplicationUser> GetBuyerByIdAsync(string userId);
    }
}
