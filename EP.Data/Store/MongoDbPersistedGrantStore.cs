using IdentityServer4.Models;
using IdentityServer4.Stores;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EP.Data.Store
{
    public sealed class MongoDbPersistedGrantStore : IPersistedGrantStore
    {
        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
        {
            throw new NotImplementedException();
        }

        public async Task<PersistedGrant> GetAsync(string key)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveAllAsync(string subjectId, string clientId)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveAsync(string key)
        {
            throw new NotImplementedException();
        }

        public async Task StoreAsync(PersistedGrant grant)
        {
            throw new NotImplementedException();
        }
    }
}
