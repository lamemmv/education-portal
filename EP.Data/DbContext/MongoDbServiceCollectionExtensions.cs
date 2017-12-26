using Microsoft.Extensions.DependencyInjection;
using System;

namespace EP.Data.DbContext
{
    public static class MongoDbServiceCollectionExtensions
    {
        public static MongoDbContextBuilder<MongoDbContext> AddMongoDbContext(
            this IServiceCollection services,
            string connectionString)
        {
            return services.AddMongoDbContext(opts =>
            {
                opts.ConnectionString = connectionString;
                // Further configuration can be used as such:
                //opts.ClientSettings = new MongoClientSettings
                //{
                //    UseSsl = true,
                //    WriteConcern = WriteConcern.WMajority,
                //    ConnectionMode = ConnectionMode.Standalone
                //};
                //opts.DatabaseSettings = new MongoDatabaseSettings
                //{
                //    ReadPreference = ReadPreference.PrimaryPreferred,
                //    WriteConcern = new WriteConcern(1)
                //};
            });
        }

        private static MongoDbContextBuilder<MongoDbContext> AddMongoDbContext(
            this IServiceCollection services,
            Action<MongoDbContextConfiguration> contextConfiguration)
        {
            services.Configure(contextConfiguration);
            var builder = new MongoDbContextBuilder<MongoDbContext>(services);

            return builder.Build();
        }
    }
}
