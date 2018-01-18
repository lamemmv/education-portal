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
    public sealed class AppUserStore<TUser> :
        IQueryableUserStore<TUser>,
        IUserClaimStore<TUser>,
        IUserLoginStore<TUser>,
        IUserRoleStore<TUser>,
        IUserPasswordStore<TUser>,
        IUserSecurityStampStore<TUser>,
        IUserTwoFactorStore<TUser>,
        IUserPhoneNumberStore<TUser>,
        IUserEmailStore<TUser>,
        IUserLockoutStore<TUser> where TUser : AppUser
    {
        private readonly IMongoCollection<TUser> _users;

        public AppUserStore(IMongoCollection<TUser> users)
        {
            _users = users;
        }

        #region IUserStore

        public IQueryable<TUser> Users => _users.AsQueryable();

        public async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
        {
            await _users.InsertOneAsync(user, cancellationToken: cancellationToken);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
        {
            if (user.Id.IsInvalidObjectId())
            {
                return IdentityResult.Failed();
            }

            var filter = Builders<TUser>.Filter.Eq(e => e.Id, user.Id);
            var result = await _users.DeleteOneAsync(filter, cancellationToken);

            return result.IsSuccess() ? IdentityResult.Success : IdentityResult.Failed();
        }

        public void Dispose()
        {
            // No need to dispose of anything, mongodb handles connection pooling automatically.
        }

        public async Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            if (userId.IsInvalidObjectId())
            {
                return null;
            }

            var filter = Builders<TUser>.Filter.Eq(e => e.Id, userId);
            var cursor = await _users.FindAsync(filter, cancellationToken: cancellationToken);

            return await cursor.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var filter = Builders<TUser>.Filter.Eq(e => e.NormalizedUserName, normalizedUserName);
            var cursor = await _users.FindAsync(filter, cancellationToken: cancellationToken);

            return await cursor.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.NormalizedUserName);
        }

        public async Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.Id);
        }

        public async Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;

            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;

            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
        {
            if (user.Id.IsInvalidObjectId())
            {
                return IdentityResult.Failed();
            }

            var filter = Builders<TUser>.Filter.Eq(e => e.Id, user.Id);
            var result = await _users.ReplaceOneAsync(filter, user, cancellationToken: cancellationToken);

            return result.IsSuccess() ? IdentityResult.Success : IdentityResult.Failed();
        }

        #endregion

        #region IUserClaimStore

        public async Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken)
        {
            IList<Claim> claims = user.Claims == null ?
                new List<Claim>() :
                user.Claims.Select(e => e.ToClaim()).ToList();
            
            return await Task.FromResult(claims);
        }

        public Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            var userClaims = user.Claims;
            userClaims = userClaims ?? new List<AppClaim>();

            userClaims.AddRange(claims.Select(clm => new AppClaim(clm.Type, clm.Value)));

            return Task.CompletedTask;
        }

        public Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            var claimCount = user.Claims?.Count;

            if (claimCount > 0)
            {
                user.Claims.RemoveAll(c =>
                    c.Type.Equals(claim.Type, StringComparison.OrdinalIgnoreCase) &&
                    c.Value.Equals(claim.Value, StringComparison.OrdinalIgnoreCase));

                user.Claims.Add(new AppClaim(newClaim.Type, newClaim.Value));
            }

            return Task.CompletedTask;
        }

        public Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            var claimCount = user.Claims?.Count;

            if (claimCount > 0)
            {
                foreach (var clm in claims)
                {
                    user.Claims.RemoveAll(c =>
                        c.Type.Equals(clm.Type, StringComparison.OrdinalIgnoreCase) &&
                        c.Value.Equals(clm.Value, StringComparison.OrdinalIgnoreCase));
                }
            }

            return Task.CompletedTask;
        }

        public async Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            var filter = Builders<TUser>.Filter.ElemMatch(e => e.Claims,
                Builders<AppClaim>.Filter.And(
                    Builders<AppClaim>.Filter.Eq(clm => clm.Type, claim.Type),
                    Builders<AppClaim>.Filter.Eq(clm => clm.Value, claim.Value)
                )
            );

            var cursor = await _users.FindAsync(filter, cancellationToken: cancellationToken);

            return await cursor.ToListAsync(cancellationToken);
        }

        #endregion

        #region IUserLoginStore

        public Task AddLoginAsync(TUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            var userLogins = user.Logins;
            userLogins = userLogins ?? new List<AppUserLogin>();

            if (!userLogins.Any(e =>
                e.LoginProvider.Equals(login.LoginProvider, StringComparison.OrdinalIgnoreCase) &&
                e.ProviderKey.Equals(login.ProviderKey, StringComparison.OrdinalIgnoreCase)))
            {
                userLogins.Add(new AppUserLogin(login.LoginProvider, login.ProviderKey, login.ProviderDisplayName));
            }

            return Task.CompletedTask;
        }

        public Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            var loginCount = user.Logins?.Count;

            if (loginCount > 0)
            {
                user.Logins.RemoveAll(e =>
                    e.LoginProvider.Equals(loginProvider, StringComparison.OrdinalIgnoreCase) &&
                    e.ProviderKey.Equals(providerKey, StringComparison.OrdinalIgnoreCase));
            }

            return Task.CompletedTask;
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user, CancellationToken cancellationToken)
        {
            var logins = user.Logins == null ?
                new List<UserLoginInfo>() :
                user.Logins.Select(e => new UserLoginInfo(e.LoginProvider, e.ProviderKey, e.ProviderDisplayName)).ToList();

            return await Task.FromResult(logins);
        }

        public async Task<TUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            var filter = Builders<TUser>.Filter.ElemMatch(e => e.Logins,
                Builders<AppUserLogin>.Filter.And(
                    Builders<AppUserLogin>.Filter.Eq(lg => lg.LoginProvider, loginProvider),
                    Builders<AppUserLogin>.Filter.Eq(lg => lg.ProviderKey, providerKey)
                )
            );

            var cursor = await _users.FindAsync(filter, cancellationToken: cancellationToken);

            return await cursor.FirstOrDefaultAsync(cancellationToken);
        }

        #endregion

        #region IUserRoleStore

        public Task AddToRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            var userRoles = user.Roles;
            userRoles = userRoles ?? new List<string>();

            if (!userRoles.Any(e => e.Equals(roleName, StringComparison.OrdinalIgnoreCase)))
            {
                userRoles.Add(roleName);
            }

            return Task.CompletedTask;
        }

        public Task RemoveFromRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            var rolesCount = user.Roles?.Count;

            if (rolesCount > 0)
            {
                user.Roles.RemoveAll(e => e.Equals(roleName, StringComparison.OrdinalIgnoreCase));
            }

            return Task.CompletedTask;
        }

        public async Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.Roles);
        }

        public async Task<bool> IsInRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            var userRoles = user.Roles;
            var isInRole = userRoles != null && userRoles.Contains(roleName);

            return await Task.FromResult(isInRole);
        }

        public async Task<IList<TUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            var filter = Builders<TUser>.Filter.ElemMatch(
                e => e.Roles,
                Builders<string>.Filter.Eq(r => r, roleName));

            var cursor = await _users.FindAsync(filter, cancellationToken: cancellationToken);

            return await cursor.ToListAsync(cancellationToken);
        }

        #endregion

        #region IUserPasswordStore

        public Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;

            return Task.CompletedTask;
        }

        public async Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.PasswordHash);
        }

        public async Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.PasswordHash != null);
        }

        #endregion

        #region IUserSecurityStampStore

        public Task SetSecurityStampAsync(TUser user, string stamp, CancellationToken cancellationToken)
        {
            user.SecurityStamp = stamp;

            return Task.CompletedTask;
        }

        public async Task<string> GetSecurityStampAsync(TUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.SecurityStamp);
        }

        #endregion

        #region IUserTwoFactorStore

        public Task SetTwoFactorEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken)
        {
            user.TwoFactorEnabled = enabled;

            return Task.CompletedTask;
        }

        public async Task<bool> GetTwoFactorEnabledAsync(TUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.TwoFactorEnabled);
        }

        #endregion

        #region IUserPhoneNumberStore

        public Task SetPhoneNumberAsync(TUser user, string phoneNumber, CancellationToken cancellationToken)
        {
            user.PhoneNumber = phoneNumber;

            return Task.CompletedTask;
        }

        public async Task<string> GetPhoneNumberAsync(TUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.PhoneNumber);
        }

        public async Task<bool> GetPhoneNumberConfirmedAsync(TUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.PhoneNumberConfirmed = confirmed;

            return Task.CompletedTask;
        }

        #endregion

        #region IUserEmailStore

        public Task SetEmailAsync(TUser user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;

            return Task.CompletedTask;
        }

        public async Task<string> GetEmailAsync(TUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.Email);
        }

        public async Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;

            return Task.CompletedTask;
        }

        public async Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            var filter = Builders<TUser>.Filter.Eq(e => e.NormalizedEmail, normalizedEmail);
            var cursor = await _users.FindAsync(filter, cancellationToken: cancellationToken);
                
            return await cursor.FirstOrDefaultAsync();
        }

        public async Task<string> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.NormalizedEmail);
        }

        public Task SetNormalizedEmailAsync(TUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;

            return Task.CompletedTask;
        }

        #endregion

        #region IUserLockoutStore

        public async Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.LockoutEnd);
        }

        public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            user.LockoutEnd = lockoutEnd;

            return Task.CompletedTask;
        }

        public async Task<int> IncrementAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
        {
            user.AccessFailedCount++;

            return await Task.FromResult(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
        {
            user.AccessFailedCount = 0;

            return Task.CompletedTask;
        }

        public async Task<int> GetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.AccessFailedCount);
        }

        public async Task<bool> GetLockoutEnabledAsync(TUser user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.LockoutEnabled);
        }

        public Task SetLockoutEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken)
        {
            user.LockoutEnabled = enabled;

            return Task.CompletedTask;
        }

        #endregion
    }
}
