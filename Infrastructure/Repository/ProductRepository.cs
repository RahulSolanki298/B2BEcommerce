﻿using Core.Entities;
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

        public async Task<bool> SaveProductList(List<ProductVM> products)
        {
            // Step 1: Fetch all related entities in bulk to avoid repeated database calls
            var colors = await _context.ProductColor.ToListAsync();
            var categories = await _context.Category.ToListAsync();
            var subCategories = await _context.SubCategory.ToListAsync();
            var clarities = await _context.ProductClarity.ToListAsync();
            var carats = await _context.ProductCarat.ToListAsync();
            var caratSizes = await _context.ProductCaratSize.ToListAsync();
            var shapes = await _context.ProductShape.ToListAsync();

            // Step 2: Create dictionaries for fast lookup by Name
            var colorDict = colors.ToDictionary(x => x.Name, x => x.Id);
            var categoryDict = categories.ToDictionary(x => x.Name, x => x.Id);
            var subCategoryDict = subCategories.ToDictionary(x => x.Name, x => x.Id);
            var clarityDict = clarities.ToDictionary(x => x.Name, x => x.Id);
            var caratDict = carats.ToDictionary(x => x.Name, x => x.Id);
            var caratSizeDict = caratSizes.ToDictionary(x => x.Name, x => x.Id);
            var shapeDict = shapes.ToDictionary(x => x.Name, x => x.Id);

            // Lists for insert and update
            var productList = new List<Product>();
            var updateList = new List<Product>();

            // Step 3: Process each product
            foreach (var product in products)
            {
                var colorId = colorDict.GetValueOrDefault(product.ColorName);
                var categoryId = categoryDict.GetValueOrDefault(product.CategoryName);
                var subCategoryId = subCategoryDict.GetValueOrDefault(product.SubCategoryName);
                var clarityId = clarityDict.GetValueOrDefault(product.ClarityName);
                var caratId = caratDict.GetValueOrDefault(product.CaratName);
                var caratSizeId = caratSizeDict.GetValueOrDefault(product.CaratSize);
                var shapeId = shapeDict.GetValueOrDefault(product.ShapeName);

                // Check if a product already exists based on related field IDs
                var existingProduct = await _context.Product
                    .Where(x => x.CategoryId == categoryId
                                && x.ClarityId == clarityId
                                && x.SubCategoryId == subCategoryId
                                && x.CaratId == caratId
                                && x.ColorId == colorId
                                && x.CaratSizeId == caratSizeId)
                    .FirstOrDefaultAsync();

                if (existingProduct != null)
                {
                    // Update existing product
                    existingProduct.Title = $"{product.CategoryName} {product.ColorName} {product.CaratName} {product.ProductType}";
                    existingProduct.Sku = product.Sku;
                    existingProduct.Price = product.Price;
                    existingProduct.UnitPrice = product.UnitPrice;
                    existingProduct.Quantity = product.Quantity;
                    existingProduct.ProductType = product.ProductType;
                    existingProduct.ShapeId = shapeId;

                    updateList.Add(existingProduct);
                }
                else
                {
                    // Insert new product
                    var newProduct = new Product
                    {
                        Title = $"{product.CategoryName} {product.ColorName} {product.CaratName} {product.ProductType}",
                        Sku = product.Sku,
                        CategoryId = categoryId,
                        SubCategoryId = subCategoryId,
                        CaratSizeId = caratSizeId,
                        CaratId = caratId,
                        ClarityId = clarityId,
                        ColorId = colorId,
                        Price = product.Price,
                        UnitPrice = product.UnitPrice,
                        Quantity = product.Quantity,
                        ProductType = product.ProductType,
                        ShapeId = shapeId
                    };

                    productList.Add(newProduct);
                }
            }

            // Step 4: Bulk insert new products and update existing products
            if (productList.Count > 0)
            {
                await _context.Product.AddRangeAsync(productList);
            }

            if (updateList.Count > 0)
            {
                _context.Product.UpdateRange(updateList);
            }

            // Step 5: Save changes to the database
            await _context.SaveChangesAsync();

            return true;
        }





    }
}
