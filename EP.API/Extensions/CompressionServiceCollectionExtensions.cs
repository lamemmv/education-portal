using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;

namespace EP.API.Extensions
{
    public static class CompressionServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomCompression(this IServiceCollection services)
        {
            return services
                .Configure<GzipCompressionProviderOptions>(opts =>
                {
                    opts.Level = CompressionLevel.Fastest;
                })
                .AddResponseCompression(opts =>
                {
                    opts.EnableForHttps = true;
                    opts.Providers.Add<GzipCompressionProvider>();
                    opts.MimeTypes = DefaultMimeTypes.Concat(CustomMimeTypes);
                });
        }

        private static IEnumerable<string> DefaultMimeTypes
        {
            get
            {
                // General.
                yield return "text/plain";
                // Static files.
                //yield return "text/css";
                //yield return "application/javascript";
                // MVC.
                //yield return "text/html";
                //yield return "application/xml";
               // yield return "text/xml";
                yield return "application/json";
                yield return "text/json";
            }
        }

        private static IEnumerable<string> CustomMimeTypes
        {
            get
            {
                yield return "image/svg+xml";
            }
        }
    }
}