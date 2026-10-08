using Catalog.Core.Entities;

namespace Catalog.Core.Repositories
{
    public interface ITypeRepository
    {
        Task<IReadOnlyList<ProductType>> GetAllTypesAsync();
        Task<ProductType> GetTypeAsync(string id);
    }
}
