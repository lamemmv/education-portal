using EP.API.Infrastructure;
using IdentityServer4.AccessTokenValidation;
using Microsoft.Extensions.DependencyInjection;

namespace EP.API.Extensions
{
    public static class AuthorizationExtensions
    {
        public static IMvcCoreBuilder AddCustomAuthorization(this IMvcCoreBuilder builder)
        {
            return builder.AddAuthorization(opts =>
            {
                opts.AddPolicy("Over21Only", policy => 
                {
                    policy.AuthenticationSchemes.Clear();
                    policy.AuthenticationSchemes.Add(IdentityServerAuthenticationDefaults.AuthenticationScheme);
                    policy.Requirements.Add(new MinimumAgeRequirement(21));
                });
            });
        }
    }
}