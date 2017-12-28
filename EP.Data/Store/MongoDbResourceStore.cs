using IdentityServer4.Models;
using IdentityServer4.Stores;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EP.Data.Store
{
    public sealed class MongoDbResourceStore : IResourceStore
    {
        private readonly IMongoCollection<ApiResource> _apiResource;

        public MongoDbResourceStore(IMongoCollection<ApiResource> apiResource)
        {
            _apiResource = apiResource;
        }

        public async Task<ApiResource> FindApiResourceAsync(string name)
        {
            var filter = Builders<ApiResource>.Filter.Eq(e => e.Name, name);
            var cursor = await _apiResource.FindAsync(filter);

            return await cursor.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(
            IEnumerable<string> scopeNames)
        {
            var filter = Builders<ApiResource>.Filter.In(e => e.Name, scopeNames);
            var cursor = await _apiResource.FindAsync(filter);

            return await cursor.ToListAsync();
        }

        public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            throw new System.NotImplementedException();
        }

        public Task<Resources> GetAllResourcesAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}