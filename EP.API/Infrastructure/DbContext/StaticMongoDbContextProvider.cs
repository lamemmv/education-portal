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
            MongoUrl url = new MongoUrl(_configuration.ConnectionString);
            IMongoClient client = _configuration.ClientSettings == null ?
                new MongoClient(url) :
                new MongoClient(_configuration.ClientSettings);

            var ctx = new TContext
            {
                MongoClient = client,
                MongoDatabase = client.GetDatabase(url.DatabaseName, _configuration.DatabaseSettings)
            };

            ctx.SetupCollections();

            return ctx;
        }

        private TContext AssignFromConnectionString(TContext ctx)
        {
            MongoUrl url = new MongoUrl(_configuration.ConnectionString);
            IMongoClient client = new MongoClient(url);

            ctx.MongoClient = client;
            ctx.MongoDatabase = client.GetDatabase(url.DatabaseName, _configuration.DatabaseSettings);

            return ctx;
        }

        private TContext AssignFromClientSettings(TContext ctx)
        {
            IMongoClient client = new MongoClient(_configuration.ClientSettings);

            ctx.MongoClient = client;

            return ctx;
        }
    }
}
