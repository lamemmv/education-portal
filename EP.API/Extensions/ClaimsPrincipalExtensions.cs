using IdentityModel;
using System.Linq;
using System.Security.Claims;
using EP.Data.Entities;

namespace EP.API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static EmbeddedUser GetEmbeddedUser(this ClaimsPrincipal user)
            => new EmbeddedUser
            {
                Id = user.GetUserId(),
                UserName = user.GetUserName(),
                ClientId = user.GetClientId()
            };

        public static string GetUserId(this ClaimsPrincipal user)
            => user.GetClaimValue(JwtClaimTypes.Subject);

        public static string GetUserName(this ClaimsPrincipal user)
            => user.GetClaimValue(JwtClaimTypes.Name);

        public static string GetClientId(this ClaimsPrincipal user)
            => user.GetClaimValue(JwtClaimTypes.ClientId);

        public static string GetClaimValue(this ClaimsPrincipal user, string claimType)
            => user.Claims?.FirstOrDefault(c => c.Type == claimType)?.Value;
    }
}