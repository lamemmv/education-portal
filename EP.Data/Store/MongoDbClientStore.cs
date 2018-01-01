using IdentityServer4.Models;
using IdentityServer4.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EP.Data.Store
{
    public sealed class MongoDbClientStore : IClientStore
    {
        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            return await Task.FromResult(Clients.FirstOrDefault(e =>
                e.ClientId.Equals(clientId, StringComparison.OrdinalIgnoreCase)));
        }

        private static IEnumerable<Client> Clients
        {
            get
            {
                yield return new Client
                {
                    ClientId = "ep.web",
                    ClientName = "EP Web Client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowOfflineAccess = true,
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
        }
    }
}