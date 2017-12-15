using EP.Data.AspNetIdentity;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace EP.API
{
    public static class StartupIdentity
    {
        public static IServiceCollection AddCustomIdentity(this IServiceCollection services)
        {
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                //.AddInMemoryPersistedGrants()
                //.AddInMemoryIdentityResources(GetIdentityResources())
                .AddInMemoryApiResources(GetApiResources())
                .AddInMemoryClients(GetClients())
                .AddAspNetIdentity<AppUser>();

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "ep.api";
                });

            return services;
        }

        private static IEnumerable<IdentityResource> GetIdentityResources()
        {
            yield return new IdentityResources.OpenId();
            yield return new IdentityResources.Profile();
            yield return new IdentityResources.Email();
            yield return new IdentityResource
            {
                Name = "role",
                UserClaims = new List<string>
                {
                    "role"
                }
            };
        }

        private static IEnumerable<ApiResource> GetApiResources()
        {
            yield return new ApiResource
            {
                Name = "ep.api",
                DisplayName = "EP API"
                // UserClaims = new List<string>
                // {
                //     "role"
                // },
                // ApiSecrets = new List<Secret>
                // {
                //     new Secret("scopeSecret".Sha256())
                // },
                // Scopes = new List<Scope>
                // {
                //     new Scope("customAPI.read"),
                //     new Scope("customAPI.write")
                // }
            };
        }

        private static IEnumerable<Client> GetClients()
        {
            yield return new Client
            {
                ClientId = "ep.web",
                ClientName = "EP Web",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                RequireConsent = false,
                ClientSecrets =
                {
                    new Secret("ep.web@P@SSW0RD".Sha256())
                },
                //RedirectUris = { "http://localhost:5002/signin-oidc" },
                //PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "ep.api"
                },
                AllowOfflineAccess = true
            };
        }
    }
}
