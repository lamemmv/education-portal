using IdentityServer4.Models;
using IdentityServer4.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EP.Data.IdentityServerStore
{
    public sealed class MongoDbClientStore : IClientStore
    {
        public async Task<Client> FindClientByIdAsync(string clientId)
            => await Task.FromResult(
                Clients.FirstOrDefault(e => e.ClientId.Equals(clientId, StringComparison.Ordinal)));

        private static IEnumerable<Client> Clients
        {
            get
            {
                yield return new Client
                {
                    ClientId = "ep.web",
                    ClientName = "Education Portal Web Client",
                    ClientSecrets =
                    {
                        new Secret("ep.web@P@SSW0RD".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowOfflineAccess = true,
                    AccessTokenLifetime = 20 * 60, // 20 mins (default is 3600 ~ 1 hour).
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    SlidingRefreshTokenLifetime = 24 * 60 * 60, // 1 day (default is 1296000 ~ 15 days).
                    AllowedScopes =
                    {
                        //IdentityServerConstants.StandardScopes.OpenId,
                        //IdentityServerConstants.StandardScopes.Profile,
                        //IdentityServerConstants.StandardScopes.Email,
                        //IdentityServerConstants.StandardScopes.OfflineAccess,
                        "ep.api.admin"
                    }
                };
            }
        }
    }
}