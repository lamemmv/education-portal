using EP.Data.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace EP.Data.AspNetIdentity
{
    public static class IdentityBuilderExtensions
    {
        private const string AspNetUserCollectionName = "AspNetUsers";
        private const string AspNetRoleCollectionName = "AspNetRoles";

        public static IdentityBuilder AddIdentityMongoStores(
            this IdentityBuilder builder,
            string connectionString)
        {
            var services = builder.Services;
            var database = MongoDbHelper.GetMongoDatabase(connectionString);

            services.AddSingleton<IUserStore<AppUser>>(p => 
            {
                var userCollection = database.GetCollection<AppUser>(AspNetUserCollectionName);
                IndexChecker.EnsureAppUserIndexes(userCollection).GetAwaiter().GetResult();

                return new AppUserStore<AppUser>(userCollection);
            });

            services.AddSingleton<IRoleStore<AppRole>>(p =>
            {
                var roleCollection = database.GetCollection<AppRole>(AspNetRoleCollectionName);
                IndexChecker.EnsureAppRoleIndex(roleCollection).GetAwaiter().GetResult();

                return new AppRoleStore<AppRole>(roleCollection);
            });

            return builder;
        }
    }
}