using EP.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EP.API.Infrastructure.DbContext
{
    public sealed class MongoDbContextBuilder<TContext> where TContext : BaseDbContext, new()
    {
        private readonly IServiceCollection _services;

        public MongoDbContextBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public MongoDbContextBuilder<TContext> Build()
        {
            _services.AddScoped(serviceProvider =>
            {
                var contextProvider = new MongoDbContextProvider<TContext>(
                    serviceProvider.GetRequiredService<IOptions<MongoDbContextConfiguration<TContext>>>(),
                    serviceProvider.GetService<IConfigureOptions<MongoDbContextConfiguration<TContext>>>());

                return contextProvider.CreateContext();
            });

            return this;
        }
    }
}
