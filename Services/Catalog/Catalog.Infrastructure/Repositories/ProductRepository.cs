using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specifications;
using Catalog.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<ProductBrand> _brands;
        private readonly IMongoCollection<ProductType> _types;
        private readonly IMongoCollection<Product> _products;

        public ProductRepository(IOptions<DatabaseSettings> databaseSettings)
        {
            var settings = databaseSettings.Value;
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _brands = database.GetCollection<ProductBrand>(settings.BrandCollectionName);
            _types = database.GetCollection<ProductType>(settings.TypeCollectionName);
            _products = database.GetCollection<Product>(settings.ProductCollectionName);
        }
        public async Task<Product> CreateProductAsync(Product product)
        {
            await _products.InsertOneAsync(product);
            return product;
        }

        public async Task<bool> DeleteProductAsync(string productId)
        {
            var deleted_product = await _products.DeleteOneAsync(p => p.Id == productId);
            return deleted_product.IsAcknowledged && deleted_product.DeletedCount > 0;
        }

        public async Task<IReadOnlyList<Product>> GetAllProductsAsync()
        {
            return await _products.Find(_ => true).ToListAsync();
        }

        public async Task<ProductBrand> GetBrandByIdAsync(string brandId)
        {
            return await _brands.Find(b => b.Id == brandId).FirstOrDefaultAsync();
        }

        public async Task<Product> GetProductAsync(string id)
        {
            return await _products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<Product>> GetProductByBrandAsync(string brandName)
        {
            return await _products.Find(p => p.Brand.Name.ToLower() == brandName.ToLower()).ToListAsync();
        }

        public async Task<IReadOnlyList<Product>> GetProductByNameAsync(string name)
        {
            var filter = Builders<Product>.Filter.Regex(p => p.Name, new BsonRegularExpression($".*{name}.*", "i"));
            return await _products.Find(filter).ToListAsync();
        }

        public async Task<Pagination<Product>> GetProductsAsync(CatalogSpecParams catalogSpecParams)
        {
            var builder = Builders<Product>.Filter;
            var filter = builder.Empty;
            if (!string.IsNullOrEmpty(catalogSpecParams.Search))
            {
                filter &= builder.Where(p => p.Name.ToLower().Contains(catalogSpecParams.Search.ToLower()));
            }
            if (!string.IsNullOrEmpty(catalogSpecParams.BrandId))
            {
                filter &= builder.Eq(p => p.Brand.Id, catalogSpecParams.BrandId);
            }
            if (!string.IsNullOrEmpty(catalogSpecParams.TypeId))
            {
                filter &= builder.Eq(p => p.Type.Id, catalogSpecParams.TypeId);
            }

            var totalItems = await _products.CountDocumentsAsync(filter);
            var data = await ApplyDataFilters(catalogSpecParams, filter);
            return new Pagination<Product>(catalogSpecParams.PageIndex, catalogSpecParams.PageSize, (int) totalItems, data);
        }

        public async Task<ProductType> GetTypeByIdAsync(string typeId)
        {
            return await _types.Find(t => t.Id == typeId).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            var updatedProduct = await _products.ReplaceOneAsync(p => p.Id == product.Id, product);
            return updatedProduct.IsAcknowledged && updatedProduct.ModifiedCount > 0;
        }

        private async Task<IReadOnlyCollection<Product>> ApplyDataFilters(CatalogSpecParams catalogSpecParams, FilterDefinition<Product> filter)
        {
            var sortDefinition = Builders<Product>.Sort.Ascending(p => p.Name);
            if (!string.IsNullOrEmpty(catalogSpecParams.Sort))
            {
                sortDefinition = catalogSpecParams.Sort switch
                {
                    "priceAsc" => Builders<Product>.Sort.Ascending(p => p.Price),
                    "priceDesc" => Builders<Product>.Sort.Descending(p => p.Price),
                    _ => Builders<Product>.Sort.Ascending(p => p.Name)
                };
            }
            return await _products
                .Find(filter)
                .Sort(sortDefinition)
                .Skip(catalogSpecParams.PageSize * (catalogSpecParams.PageIndex - 1))
                .Limit(catalogSpecParams.PageSize)
                .ToListAsync();
        }
    }
}
