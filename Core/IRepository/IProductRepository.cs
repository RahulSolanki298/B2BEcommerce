using Core.Entities;

namespace Core.IRepository
{
    public interface IProductRepository
    {
        Task<IReadOnlyList<Product>> GetProductList();

        Task<Product> GetProductById(Guid id);
    }
}
