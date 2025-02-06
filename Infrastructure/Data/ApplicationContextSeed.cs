using Core.Entities;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Text.Json;

namespace Infrastructure.Data
{
    public class ApplicationContextSeed
    {
        public static async Task SeedAsync(ApplicationDBContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                if (!context.ProductColor.Any())
                {
                    var colorDatas = File.ReadAllText(path + @"/SeedData/Color.json");
                    var colors = JsonSerializer.Deserialize<List<ProductColor>>(colorDatas);

                    foreach (var item in colors)
                    {
                        context.ProductColor.Add(item);
                    }

                    await context.SaveChangesAsync();
                }

                if (!context.ProductCarat.Any())
                {
                    var caratsData = File.ReadAllText(path + @"/SeedData/Carat.json");
                    var carats = JsonSerializer.Deserialize<List<ProductCarat>>(caratsData);

                    foreach (var item in carats)
                    {
                        context.ProductCarat.Add(item);
                    }

                    await context.SaveChangesAsync();
                }

                if (!context.ProductShape.Any())
                {
                    var shapeData = File.ReadAllText(path + @"/SeedData/Shape.json");
                    var shapes = JsonSerializer.Deserialize<List<ProductShapes>>(shapeData);

                    foreach (var item in shapes)
                    {
                        context.ProductShape.Add(item);
                    }

                    await context.SaveChangesAsync();
                }

                if (!context.ProductClarity.Any())
                {
                    var clarityData = File.ReadAllText(path + @"/SeedData/Clarity.json");
                    var clarities = JsonSerializer.Deserialize<List<ProductClarity>>(clarityData);

                    foreach (var item in clarities)
                    {
                        context.ProductClarity.Add(item);
                    }

                    await context.SaveChangesAsync();
                }

                if (!context.Product.Any())
                {
                    var productsData = File.ReadAllText(path + @"/Data/SeedData/products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                    foreach (var item in products)
                    {
                        context.Product.Add(item);
                    }

                    await context.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<ApplicationContextSeed>();
                logger.LogError(ex.Message);
            }
        }



    }
}
