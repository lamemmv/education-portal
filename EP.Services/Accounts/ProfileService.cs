using EP.Data.AspNetIdentity;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EP.Services.Accounts
{
    public sealed class ProfileService : IProfileService
    {
        private readonly RoleManager<AppRole> _roleManger;
        private readonly UserManager<AppUser> _userManger;

        public ProfileService(
            RoleManager<AppRole> roleManger,
            UserManager<AppUser> userManger)
        {
            _roleManger = roleManger;
            _userManger = userManger;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subjectId = context.Subject.GetSubjectId();
            var user = await _userManger.FindByIdAsync(subjectId);

            context.IssuedClaims = await GetIssuedClaims(user);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            bool isActive = false;
            var subjectId = context?.Subject?.GetSubjectId();

            if (!string.IsNullOrEmpty(subjectId))
            {
                var user = await _userManger.FindByIdAsync(subjectId);

                isActive = user != null && !await _userManger.IsLockedOutAsync(user);
            }

            context.IsActive = isActive;
        }

        private async Task<List<Claim>> GetIssuedClaims(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Name, await _userManger.GetUserNameAsync(user))
            };

            if (_userManger.SupportsUserEmail)
            {
                var email = await _userManger.GetEmailAsync(user);

                if (!string.IsNullOrEmpty(email))
                {
                    claims.AddRange(new[]
                    {
                        new Claim(JwtClaimTypes.Email, email),
                        new Claim(
                            JwtClaimTypes.EmailVerified,
                            await _userManger.IsEmailConfirmedAsync(user) ? "true" : "false",
                            ClaimValueTypes.Boolean)
                    });
                }
            }

            if (_userManger.SupportsUserPhoneNumber)
            {
                var phoneNumber = await _userManger.GetPhoneNumberAsync(user);

                if (!string.IsNullOrEmpty(phoneNumber))
                {
                    claims.AddRange(new[]
                    {
                        new Claim(JwtClaimTypes.PhoneNumber, phoneNumber),
                        new Claim(
                            JwtClaimTypes.PhoneNumberVerified,
                            await _userManger.IsPhoneNumberConfirmedAsync(user) ? "true" : "false",
                            ClaimValueTypes.Boolean)
                    });
                }
            }

            if (_userManger.SupportsUserClaim)
            {
                claims.AddRange(await _userManger.GetClaimsAsync(user));
            }

            if (_userManger.SupportsUserRole)
            {
                var roles = await _userManger.GetRolesAsync(user);

                claims.AddRange(roles.Select(role => new Claim(JwtClaimTypes.Role, role)));

                foreach (var roleName in roles)
                {
                    var role = await _roleManger.FindByNameAsync(roleName.ToUpperInvariant());

                    if (role != null)
                    {
                        claims.AddRange(await _roleManger.GetClaimsAsync(role));
                    }
                }
            }

            return claims;
        }
    }
}