using Core.Entities;
using Core.IRepository;
using Core.ViewModels;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDBContext _context;

        public ProductRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Product>> GetProductList() => await _context.Product.ToListAsync();

        public async Task<Product> GetProductById(Guid id) => await _context.Product.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<bool> AddProductList(List<ProductVM> products)
        {
            var productList = new List<Product>();

            foreach (var product in products) { 
                var colorId= _context.ProductColor.Where(x=>x.Name==product.ColorName).FirstOrDefault();
                var categoryId= _context.Category.Where(x => x.Name == product.ColorName).FirstOrDefault();
                var subCategoryId= _context.SubCategory.Where(x => x.Name == product.SubCategoryName).FirstOrDefault();
                var clarityId= _context.ProductClarity.Where(x => x.Name == product.ClarityName).FirstOrDefault();
                var caratId= _context.ProductCarat.Where(x => x.Name == product.CaratName).FirstOrDefault();
                var catetsizeId= _context.ProductCaratSize.Where(x => x.Name == product.CaratSize).FirstOrDefault();

                var productDT= _context.Product.Where(x=>x.CategoryId.HasValue).FirstOrDefault(); 

            }


            return false;
        }




    }
}
