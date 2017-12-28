using MongoDB.Driver;
using System.Threading.Tasks;

namespace EP.Data.AspNetIdentity
{
    public static class IndexChecker
    {
        public async static Task EnsureAppRoleIndex(IMongoCollection<AppRole> roles)
        {
            var roleName = Builders<AppRole>.IndexKeys.Ascending(t => t.NormalizedName);
            var unique = new CreateIndexOptions { Unique = true };

            await roles.Indexes.CreateOneAsync(roleName, unique);
        }

        public async static Task EnsureAppUserIndexes(IMongoCollection<AppUser> users)
		{
            await Task.WhenAll(
                EnsureUniqueIndexOnNormalizedUserName(users),
                EnsureUniqueIndexOnNormalizedEmail(users)
            );
		}

        private async static Task EnsureUniqueIndexOnNormalizedUserName(IMongoCollection<AppUser> users)
		{
			var userName = Builders<AppUser>.IndexKeys.Ascending(e => e.NormalizedUserName);
			var unique = new CreateIndexOptions { Unique = true };
			
            await users.Indexes.CreateOneAsync(userName, unique);
		}

        private async static Task EnsureUniqueIndexOnNormalizedEmail(IMongoCollection<AppUser> users)
		{
			var email = Builders<AppUser>.IndexKeys.Ascending(t => t.NormalizedEmail);
			var unique = new CreateIndexOptions { Unique = true };

			await users.Indexes.CreateOneAsync(email, unique);
		}       
    }
}