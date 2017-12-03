using EP.Data;
using MongoDB.Driver;

namespace EP.API.Infrastructure.DbContext
{
    public sealed class StaticMongoDbContextProvider<TContext> where TContext : BaseDbContext, new()
    {
        private readonly MongoDbContextConfiguration _configuration;

        public StaticMongoDbContextProvider(MongoDbContextConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TContext CreateContext()
        {
            var ctx = new TContext();

            ctx.MongoClient = GetMongoClient();
            ctx.MongoDatabase = GetMongoDatabase(ctx.MongoClient);

            ctx.SetupCollections();

            return ctx;
        }

        private IMongoClient GetMongoClient()
        {
            if (_configuration.ConnectionString != null && _configuration.DatabaseName != null)
            {
                return new MongoClient(_configuration.ConnectionString);
            }

            if (_configuration.ClientSettings != null)
            {
                return new MongoClient(_configuration.ClientSettings);
            }

            return null;
        }

        private IMongoDatabase GetMongoDatabase(IMongoClient client)
        {
            return client.GetDatabase(_configuration.DatabaseName, _configuration.DatabaseSettings);
        }
    }
}
