using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.Extensions.DependencyInjection;

namespace EP.Data.IdentityServerStore
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

        public static IIdentityServerBuilder AddMongoDbProfileService(
            this IIdentityServerBuilder builder)
        {
            builder.Services.AddScoped<IProfileService, ProfileService>();
            
            return builder.AddProfileService<ProfileService>();
        }
    }
}