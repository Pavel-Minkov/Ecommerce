using Catalog.Core.Entities;
using Catalog.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data
{
    public class DatabaseSeeder
    {
        public static async Task SeedAsync(IOptions<DatabaseSettings> options)
        {
            var setting = options.Value;
            var client = new MongoClient(setting.ConnectionString);
            var database = client.GetDatabase(setting.DatabaseName);
            var brands = database.GetCollection<ProductBrand>(setting.BrandCollectionName);
            var types = database.GetCollection<ProductType>(setting.TypeCollectionName);
            var products = database.GetCollection<Product>(setting.ProductCollectionName);

            var seedBasePath = Path.Combine("Data", "SeedData");
            List<ProductBrand> brandList = [];
            List<ProductType> typeList = [];
            List<Product> productList = [];
            if ((await brands.CountDocumentsAsync(_ => true)) == 0)
            {
                var brandData = await File.ReadAllTextAsync(Path.Combine(seedBasePath, "brands.json"));
                brandList = JsonSerializer.Deserialize<List<ProductBrand>>(brandData) ?? [];
                await brands.InsertManyAsync(brandList);
            }
            else
            {
                brandList = await brands.Find(_ => true).ToListAsync();
            }
            if ((await types.CountDocumentsAsync(_ => true)) == 0)
            {
                var typeData = await File.ReadAllTextAsync(Path.Combine(seedBasePath, "types.json"));
                typeList = JsonSerializer.Deserialize<List<ProductType>>(typeData) ?? [];
                await types.InsertManyAsync(typeList);
            }
            else
            {
                typeList = await types.Find(_ => true).ToListAsync();
            }
            if ((await products.CountDocumentsAsync(_ => true)) == 0)
            {
                var productData = await File.ReadAllTextAsync(Path.Combine(seedBasePath, "products.json"));
                productList = JsonSerializer.Deserialize<List<Product>>(productData) ?? [];
                foreach (Product product in productList)
                {
                    //Reset ID so Mongo DB to generate one
                    product.Id = null!;
                    if (product.CreatedDate == default)
                    {
                        product.CreatedDate = DateTimeOffset.UtcNow;
                    }
                }
                await products.InsertManyAsync(productList);
            }
            else
            {
                productList = await products.Find(_ => true).ToListAsync();
            }
        }
    }
}
