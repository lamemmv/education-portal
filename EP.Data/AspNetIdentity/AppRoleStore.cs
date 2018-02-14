using EP.Data.Extensions;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace EP.Data.AspNetIdentity
{
    public sealed class AppRoleStore<TRole> :
        IRoleClaimStore<TRole>,
        IQueryableRoleStore<TRole> where TRole : AppRole
    {
        private readonly IMongoCollection<TRole> _roles;

        public AppRoleStore(IMongoCollection<TRole> roles)
        {
            _roles = roles;
        }

        #region IRoleClaimStore

        public async Task<IList<Claim>> GetClaimsAsync(TRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            var claims = role.Claims == null ?
                new List<Claim>() :
                role.Claims.Select(e => e.ToClaim()).ToList();

            return await Task.FromResult(claims);
        }

        public Task AddClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default(CancellationToken))
        {
            var roleClaims = role.Claims;
            roleClaims = roleClaims ?? new List<AppClaim>();

            roleClaims.Add(new AppClaim(claim.Type, claim.Value));

            return Task.CompletedTask;
        }

        public Task RemoveClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default(CancellationToken))
        {
            var claimCount = role.Claims?.Count;

            if (claimCount > 0)
            {
                role.Claims.RemoveAll(c =>
                    c.Type.Equals(claim.Type, StringComparison.OrdinalIgnoreCase) &&
                    c.Value.Equals(claim.Value, StringComparison.OrdinalIgnoreCase));
            }

            return Task.CompletedTask;
        }

        #endregion

        #region IQueryableRoleStore

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
            => await Task.FromResult(role.NormalizedName);

        public async Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
            => await Task.FromResult(role.Id);

        public async Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken)
            => await Task.FromResult(role.Name);

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

        #endregion
    }
}
