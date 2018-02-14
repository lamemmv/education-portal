using IdentityServer4.Models;
using IdentityServer4.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EP.Data.IdentityServerStore
{
    public sealed class MongoDbResourceStore : IResourceStore
    {
        public async Task<ApiResource> FindApiResourceAsync(string name)
            => await Task.FromResult(
                ApiResources.FirstOrDefault(e => e.Name.Equals(name, StringComparison.Ordinal)));

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
            => await Task.FromResult(ApiResources.Where(e => scopeNames.Contains(e.Name)));

        public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
            => await Task.FromResult(IdentityResources.Where(e => scopeNames.Contains(e.Name)));

        public async Task<Resources> GetAllResourcesAsync()
            => await Task.FromResult(new Resources(IdentityResources, ApiResources));

        private static IEnumerable<ApiResource> ApiResources
        {
            get
            {
                yield return new ApiResource("ep.api.admin", "Education Portal API Administration");
            }
        }

        private static IEnumerable<IdentityResource> IdentityResources
        {
            get
            {
                //yield return new IdentityResources.OpenId();
                //yield return new IdentityResources.Profile();
                //yield return new IdentityResources.Email();
                //yield return new IdentityResource { Name = "ep.api.admin" };
                yield break;
            }
        }
    }
}