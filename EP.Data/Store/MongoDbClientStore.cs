using EP.Data.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EP.Data.Store
{
    public sealed class MongoDbClientStore : IClientStore
    {
        private readonly IMongoCollection<Client> _client;
        
        public MongoDbClientStore(IMongoCollection<Client> client)
        {
            _client = client;
        }

        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            var filter = Builders<Client>.Filter.Eq(e => e.ClientId, clientId);
            var cursor = await _client.FindAsync(filter);

            return await cursor.FirstOrDefaultAsync();
        }

        internal async Task<long> CountAsync()
        {
            var filter = Builders<Client>.Filter.Empty;

            return await _client.CountAsync(filter);
        }

        internal async Task CreateAsync(IEnumerable<Client> clients)
        {
            await _client.InsertManyAsync(clients);
        }

        internal async Task<bool> DeleteAsync()
        {
            var filter = Builders<Client>.Filter.Empty;
            var result = await _client.DeleteManyAsync(filter);

            return result.IsSuccess();
        }
    }
}