using EP.Data.DbContext;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;

namespace EP.Data.Store
{
    public static class IdentityServerExtensions
    {
        private const string ClientCollectionName = "Clients";

        public static IIdentityServerBuilder AddMongoDbClients(
            this IIdentityServerBuilder builder,
            string connectionString)
        {
            var services = builder.Services;

            services.AddScoped<IClientStore>(p =>
            {
                var database = MongoDbHelper.GetMongoDatabase(connectionString);
                var clientCollection = database.GetCollection<Client>(ClientCollectionName);

                return new MongoDbClientStore(clientCollection);
            });

            services.AddScoped<ICorsPolicyService, InMemoryCorsPolicyService>();

            return builder;
        }
    }
}