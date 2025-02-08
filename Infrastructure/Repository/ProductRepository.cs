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
            var productItem = new Product();
            foreach (var product in products) { 
                var colorId= _context.ProductColor.Where(x=>x.Name==product.ColorName).FirstOrDefault();
                var categoryId= _context.Category.Where(x => x.Name == product.CategoryName).FirstOrDefault();
                var subCategoryId= _context.SubCategory.Where(x => x.Name == product.SubCategoryName).FirstOrDefault();
                var clarityId= _context.ProductClarity.Where(x => x.Name == product.ClarityName).FirstOrDefault();
                var caratId= _context.ProductCarat.Where(x => x.Name == product.CaratName).FirstOrDefault();
                var caratSizeId= _context.ProductCaratSize.Where(x => x.Name == product.CaratSize).FirstOrDefault();
                var shapeId= _context.ProductShape.Where(x => x.Name == product.ShapeName).FirstOrDefault();

                var productDT= _context.Product.Where(x=>x.CategoryId.Equals(categoryId) 
                                    && x.ClarityId.Equals(clarityId)
                                    && x.SubCategoryId.Equals(subCategoryId)
                                    && x.CaratId.Equals(caratId) 
                                    && x.ColorId.Equals(colorId)
                                    && x.CaratSizeId.Equals(caratSizeId)).FirstOrDefault();

                if (productDT != null) 
                {
                    
                }
                else
                {
                    productItem.Title = product.CategoryName + " " + product.ColorName + " " + product.CaratName + " " + product.ProductType;
                    productItem.Sku= product.Sku;
                    productItem.CategoryId = Convert.ToInt32(categoryId);
                    productItem.SubCategoryId = Convert.ToInt32(subCategoryId);
                    productItem.CaratSizeId = Convert.ToInt32(caratSizeId);
                    productItem.CaratId = Convert.ToInt32(caratId);
                    productItem.ClarityId = Convert.ToInt32(clarityId);
                    productItem.ColorId = Convert.ToInt32(colorId);
                    productItem.Price= product.Price;
                    productItem.UnitPrice = product.UnitPrice;
                    productItem.Quantity = product.Quantity;
                    productItem.ProductType = product.ProductType;
                    productItem.ShapeId = Convert.ToInt32(shapeId);


                    productList.Add(productItem);
                   
                }

            }

            await _context.Product.AddRangeAsync(productList);
            return false;
        }




    }
}
