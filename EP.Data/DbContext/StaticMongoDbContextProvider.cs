using MongoDB.Driver;

namespace EP.Data.DbContext
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
    }
}
