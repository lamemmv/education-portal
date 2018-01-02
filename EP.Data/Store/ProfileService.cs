using EP.Data.AspNetIdentity;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;
using IdentityModel;

namespace EP.Data.Store
{
    public sealed class ProfileService : IProfileService
    {
        private readonly UserManager<AppUser> _userManger;
        
        public ProfileService(UserManager<AppUser> userManger)
        {
            _userManger = userManger;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subjectId = context.Subject.GetSubjectId();
            var user = await _userManger.FindByIdAsync(subjectId);

            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Role, string.Join(',', user.Roles)),
                new Claim(JwtClaimTypes.Name, user.UserName),
                new Claim(JwtClaimTypes.Email, user.Email)
            };

            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var subjectId = context.Subject.GetSubjectId();
            var user = await _userManger.FindByIdAsync(subjectId);
            
            context.IsActive = user != null;
        }
    }
}