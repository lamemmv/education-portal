using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.Extensions.DependencyInjection;

namespace EP.Data.Store
{
    public static class IdentityServerBuilderExtensions
    {
        public static IIdentityServerBuilder AddMongoDbClients(
            this IIdentityServerBuilder builder)
        {
            var services = builder.Services;

            services.AddSingleton<IClientStore, MongoDbClientStore>();
            services.AddScoped<ICorsPolicyService, InMemoryCorsPolicyService>();

            return builder;
        }

        public static IIdentityServerBuilder AddMongoDbIdentityApiResources(
            this IIdentityServerBuilder builder)
        {
            builder.Services.AddSingleton<IResourceStore, MongoDbResourceStore>();

            return builder;
        }

        public static IIdentityServerBuilder AddMongoDbPersistedGrants(
            this IIdentityServerBuilder builder)
        {
            builder.Services.AddSingleton<IPersistedGrantStore, MongoDbPersistedGrantStore>();

            return builder;
        }
    }
}