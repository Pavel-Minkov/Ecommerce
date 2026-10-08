using Catalog.Core.Entities;
using Catalog.Core.Specifications;

namespace Catalog.Core.Repositories
{
    public interface IProductRepository
    {
        Task<IReadOnlyList<Product>> GetAllProductsAsync();
        Task<Pagination<Product>> GetProductsAsync(CatalogSpecParams catalogSpecParams);
        Task<IReadOnlyList<Product>> GetProductByNameAsync(string name);
        Task<IReadOnlyList<Product>> GetProductByBrandAsync(string brandName);
        Task<Product> GetProductAsync(string id);
        Task<Product> CreateProductAsync(Product product);
        Task<bool> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(string productId);
        Task<ProductBrand> GetBrandByIdAsync(string brandId);
        Task<ProductType> GetTypeByIdAsync(string typeId);
    }
}
