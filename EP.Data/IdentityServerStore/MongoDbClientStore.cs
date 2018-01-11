using IdentityServer4.Models;
using IdentityServer4.Stores;
using IdentityServer4;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace EP.Data.IdentityServerStore
{
    public sealed class MongoDbClientStore : IClientStore
    {
        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            return await Task.FromResult(
                Clients.FirstOrDefault(e => e.ClientId.Equals(clientId, StringComparison.Ordinal)));
        }

        private static IEnumerable<Client> Clients
        {
            get
            {
                yield return new Client
                {
                    ClientId = "ep.web",
                    ClientName = "Education Portal Web Client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowOfflineAccess = true,
                    ClientSecrets =
                    {
                        new Secret("ep.web@P@SSW0RD".Sha256())
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "ep.api.admin"
                    }
                };
            }
        }
    }
}