using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class TypeRepository : ITypeRepository
    {
        private readonly IMongoCollection<ProductType> _types;

        public TypeRepository(IOptions<DatabaseSettings> databaseSettings)
        {
            var settings = databaseSettings.Value;
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _types = database.GetCollection<ProductType>(settings.TypeCollectionName);
        }

        public async Task<IReadOnlyList<ProductType>> GetAllTypesAsync()
        {
            return await _types.Find(_ => true).ToListAsync();
        }

        public async Task<ProductType> GetTypeAsync(string id)
        {
            return await _types.Find(t => t.Id == id).FirstOrDefaultAsync();
        }
    }
}
