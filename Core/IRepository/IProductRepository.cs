using Core.Entities;
using Core.ViewModels;

namespace Core.IRepository
{
    public interface IProductRepository
    {
        Task<IReadOnlyList<Product>> GetProductList();

        Task<Product> GetProductById(Guid id);

        Task<bool> SaveProductList(List<ProductVM> products);
    }
}
