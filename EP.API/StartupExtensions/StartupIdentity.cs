using EP.Data.AspNetIdentity;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using IdentityServer4.Test;
using System.Security.Claims;
using IdentityModel;
using System.Linq;

namespace EP.API
{
    public static class StartupIdentity
    {
        public static IServiceCollection AddCustomIdentity(this IServiceCollection services)
        {
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(GetApiResources())
                .AddInMemoryClients(GetClients())
                .AddTestUsers(GetUsers().ToList());

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "ep.api";
                });

            return services;
        }

        private static IEnumerable<ApiResource> GetApiResources()
        {
            yield return new ApiResource("ep.api", "EP API");
        }

        private static IEnumerable<Client> GetClients()
        {
            yield return new Client
            {
                ClientId = "ep.web",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                RequireConsent = false,
                ClientSecrets =
                {
                    new Secret("ep.web@P@SSW0RD".Sha256())
                },
                AllowedScopes =
                {
                    "ep.api"
                }
            };
        }

        private static IEnumerable<TestUser> GetUsers()
        {
            // yield return new TestUser
            // {
            //     SubjectId = "1",
            //     Username = "alice",
            //     Password = "password"
            // };
            yield return new TestUser
            {
                SubjectId = "5BE86359-073C-434B-AD2D-A3932222DABE",
                Username = "scott",
                Password = "password",
                Claims = new List<Claim> 
                {
                    new Claim(JwtClaimTypes.Email, "scott@scottbrady91.com"),
                    new Claim(JwtClaimTypes.Role, "admin")
                }
            };
        }
    }
}
