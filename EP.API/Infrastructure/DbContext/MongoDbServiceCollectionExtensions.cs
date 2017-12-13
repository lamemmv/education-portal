using Microsoft.Extensions.DependencyInjection;
using System;
using EP.Data.DbContext;

namespace EP.API.Infrastructure.DbContext
{
    public static class MongoDbServiceCollectionExtensions
    {
        public static MongoDbContextBuilder<TContext> AddMongoDbContext<TContext>(
            this IServiceCollection services,
            Action<MongoDbContextConfiguration<TContext>> contextConfiguration) where TContext : BaseDbContext, new()
        {
            services.Configure(contextConfiguration);
            var builder = new MongoDbContextBuilder<TContext>(services);

            return builder.Build();
        }
    }
}
