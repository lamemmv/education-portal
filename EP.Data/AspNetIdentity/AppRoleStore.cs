using EP.Data.Extensions;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace EP.Data.AspNetIdentity
{
    public sealed class AppRoleStore<TRole> : IQueryableRoleStore<TRole>
        where TRole : AppRole
    {
        private readonly IMongoCollection<TRole> _roles;

        public AppRoleStore(IMongoCollection<TRole> roles)
        {
            _roles = roles;
            EnsureUniqueIndexOnNormalizedEmail(roles).GetAwaiter().GetResult();
        }

        public IQueryable<TRole> Roles => _roles.AsQueryable();

        public async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken)
        {
            await _roles.InsertOneAsync(role, cancellationToken: cancellationToken);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken)
        {
            if (role.Id.IsInvalidObjectId())
            {
                return IdentityResult.Failed();
            }

            var filter = Builders<TRole>.Filter.Eq(e => e.Id, role.Id);
            var result = await _roles.DeleteOneAsync(filter, cancellationToken);

            return result.IsSuccess() ? IdentityResult.Success : IdentityResult.Failed();
        }

        public void Dispose()
        {
            // No need to dispose of anything, mongodb handles connection pooling automatically.
        }

        public async Task<TRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            if (roleId.IsInvalidObjectId())
            {
                return null;
            }

            var filter = Builders<TRole>.Filter.Eq(e => e.Id, roleId);
            var cursor = await _roles.FindAsync(filter, cancellationToken: cancellationToken);

            return await cursor.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<TRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            var filter = Builders<TRole>.Filter.Eq(e => e.NormalizedName, normalizedRoleName);
            var cursor = await _roles.FindAsync(filter, cancellationToken: cancellationToken);

            return await cursor.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            return await Task.FromResult(role.NormalizedName);
        }

        public async Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
        {
            return await Task.FromResult(role.Id);
        }

        public async Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            return await Task.FromResult(role.Name);
        }

        public Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;

            return Task.CompletedTask;
        }

        public Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;

            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken)
        {
            if (role.Id.IsInvalidObjectId())
            {
                return IdentityResult.Failed();
            }

            var filter = Builders<TRole>.Filter.Eq(e => e.Id, role.Id);
            var result = await _roles.ReplaceOneAsync(filter, role, cancellationToken: cancellationToken);

            return result.IsSuccess() ? IdentityResult.Success : IdentityResult.Failed();
        }

        private async static Task EnsureUniqueIndexOnNormalizedEmail(IMongoCollection<TRole> roles)
		{
            var roleName = Builders<TRole>.IndexKeys.Ascending(t => t.NormalizedName);
			var unique = new CreateIndexOptions { Unique = true };

			await roles.Indexes.CreateOneAsync(roleName, unique);
		}
    }
}
