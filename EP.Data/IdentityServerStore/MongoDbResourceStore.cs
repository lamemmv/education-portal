using IdentityServer4.Models;
using IdentityServer4.Stores;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace EP.Data.IdentityServerStore
{
    public sealed class MongoDbResourceStore : IResourceStore
    {
        public async Task<ApiResource> FindApiResourceAsync(string name)
        {
            return await Task.FromResult(
                ApiResources.FirstOrDefault(e => e.Name.Equals(name, StringComparison.Ordinal)));
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(
            IEnumerable<string> scopeNames)
        {
            return await Task.FromResult(ApiResources.Where(e => scopeNames.Contains(e.Name)));
        }

        public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(
            IEnumerable<string> scopeNames)
        {
            return await Task.FromResult(IdentityResources.Where(e => scopeNames.Contains(e.Name)));
        }

        public async Task<Resources> GetAllResourcesAsync()
        {
            return await Task.FromResult(new Resources(IdentityResources, ApiResources));
        }

        private static IEnumerable<ApiResource> ApiResources
        {
            get
            {
                yield return new ApiResource("ep.api", "Education Portal API");
            }
        }

        private static IEnumerable<IdentityResource> IdentityResources
        {
            get
            {
                //yield return new IdentityResource.OpenId();
                //yield return new IdentityResource.Profile();
                yield break;
            }
        }
    }
}