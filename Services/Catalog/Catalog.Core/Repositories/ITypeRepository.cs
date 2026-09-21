using Catalog.Core.Entities;

namespace Catalog.Core.Repositories
{
    public interface ITypeRepository
    {
        Task<IEnumerable<ProductType>> GetAllTypesAsync();
        Task<ProductType> GetTypeAsync(string id);
    }
}
