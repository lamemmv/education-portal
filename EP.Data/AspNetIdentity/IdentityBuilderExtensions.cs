using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace EP.Data.AspNetIdentity
{
    public static class IdentityBuilderExtensions
    {
		private const string AspNetUsersCollectionName = "aspnetusers";
		private const string AspNetRolesCollectionName = "aspnetroles";

        public static IdentityBuilder AddIdentityWithMongoStores(this IServiceCollection services, string connectionString)
		{
			return AddIdentityWithMongoStores<AppUser, AppRole>(services, connectionString);
		}

        private static IdentityBuilder AddIdentityWithMongoStores<TUser, TRole>(
            IServiceCollection services,
            string connectionString) where TUser : AppUser where TRole : AppRole
		{
			IMongoDatabase database = GetDatabase(connectionString);
			IdentityBuilder builder = services.AddIdentity<TUser, TRole>();

			IMongoCollection<TUser> usersCollection = database.GetCollection<TUser>(AspNetUsersCollectionName);
			builder.Services.AddSingleton<IUserStore<TUser>>(p => new AppUserStore<TUser>(usersCollection));
			
			IMongoCollection<TRole> rolesCollection = database.GetCollection<TRole>(AspNetRolesCollectionName);
			builder.Services.AddSingleton<IRoleStore<TRole>>(p => new AppRoleStore<TRole>(rolesCollection));
			
			return builder;
		}

		private static IMongoDatabase GetDatabase(string connectionString)
		{
			MongoUrl url = new MongoUrl(connectionString);
			IMongoClient client = new MongoClient(url);
			
			return client.GetDatabase(url.DatabaseName);
		}
    }
}