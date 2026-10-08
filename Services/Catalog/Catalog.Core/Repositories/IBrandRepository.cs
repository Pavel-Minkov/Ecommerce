using Catalog.Core.Entities;

namespace Catalog.Core.Repositories
{
    public interface IBrandRepository
    {
        Task<IReadOnlyList<ProductBrand>> GetAllBrandsAsync();
        Task<ProductBrand> GetBrandAsync(string id);
    }
}
