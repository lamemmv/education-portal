using Microsoft.Extensions.Options;

namespace EP.Data.DbContext
{
    public sealed class MongoDbContextProvider<TContext> where TContext : BaseDbContext, new()
    {
        private readonly MongoDbContextConfiguration _configuration;

        public MongoDbContextProvider(
            IOptions<MongoDbContextConfiguration<TContext>> options,
            IConfigureOptions<MongoDbContextConfiguration<TContext>> configure)
        {
            _configuration = options.Value;
            var configOptions = configure as ConfigureOptions<MongoDbContextConfiguration<TContext>>;

            if (configOptions != null)
            {
                configOptions.Configure(options.Value);
            }
        }

        public TContext CreateContext()
        {
            var ctx = new TContext
            {
                MongoDatabase = MongoDbHelper.GetMongoDatabase(
                    _configuration.ConnectionString,
                    _configuration.ClientSettings,
                    _configuration.DatabaseSettings)
            };

            ctx.SetupCollections();

            return ctx;
        }
    }
}
