using EP.Data.Repositories;
using Microsoft.Extensions.Options;

namespace EP.Data
{
    public sealed class MongoDbContextProvider<TContext> where TContext : BaseDbContext, new()
    {
        private readonly StaticMongoDbContextProvider<TContext> _staticProvider;

        public MongoDbContextProvider(
            IOptions<MongoDbContextConfiguration<TContext>> options,
            IConfigureOptions<MongoDbContextConfiguration<TContext>> configure)
        {
            MongoDbContextConfiguration config = options.Value;
            var configOptions = configure as ConfigureOptions<MongoDbContextConfiguration<TContext>>;

            if (configOptions != null)
            {
                configOptions.Configure(options.Value);
            }

            _staticProvider = new StaticMongoDbContextProvider<TContext>(config);
        }

        public TContext CreateContext()
        {
            return _staticProvider.CreateContext();
        }
    }
}
