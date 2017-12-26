using EP.Data.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace EP.Data.AspNetIdentity
{
    public static class IdentityBuilderExtensions
    {
        private const string AspNetUsersCollectionName = "AspNetUsers";
        private const string AspNetRolesCollectionName = "AspNetRoles";

        public static IdentityBuilder AddIdentityMongoStores(
            this IdentityBuilder builder,
            string connectionString)
        {
            var database = MongoDbHelper.GetMongoDatabase(connectionString);

            var usersCollection = database.GetCollection<AppUser>(AspNetUsersCollectionName);
            builder.Services.AddSingleton<IUserStore<AppUser>>(p => new AppUserStore<AppUser>(usersCollection));

            var rolesCollection = database.GetCollection<AppRole>(AspNetRolesCollectionName);
            builder.Services.AddSingleton<IRoleStore<AppRole>>(p => new AppRoleStore<AppRole>(rolesCollection));

            return builder;
        }
    }
}