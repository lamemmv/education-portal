using EP.API.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace EP.API.Extensions
{
    public static class AuthorizationExtensions
    {
        public static IMvcCoreBuilder AddCustomAuthorization(this IMvcCoreBuilder builder)
        {
            return builder.AddAuthorization(opts =>
            {
                opts.AddPolicy("blobmanager", policy => policy.Requirements.Add(ContactOperations.BlobManagerOperationName));
            });
        }
    }
}