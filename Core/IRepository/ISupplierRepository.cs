using Core.Entities;

namespace Core.IRepository
{
    public interface ISupplierRepository
    {
        Task<IReadOnlyList<ApplicationUser>> GetSuppierListAsync();

        Task<ApplicationUser> GetSupplierByIdAsync(string userId);
    }
}
