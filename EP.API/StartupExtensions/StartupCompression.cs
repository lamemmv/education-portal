using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.IO.Compression;

namespace EP.API.StartupExtensions
{
    public static class StartupCompression
    {
        public static IServiceCollection AddCustomCompression(this IServiceCollection services)
        {
            return services
                .AddResponseCompression(opts =>
                {
                    opts.EnableForHttps = true;
                    opts.Providers.Add<GzipCompressionProvider>();
                    opts.MimeTypes = MimeTypes;
                })
                .Configure<GzipCompressionProviderOptions>(opts =>
                {
                    opts.Level = CompressionLevel.Fastest;
                });
        }

        private static IEnumerable<string> MimeTypes
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
                //yield return "text/xml";
                yield return "application/json";
                yield return "text/json";
                // Custom.
                yield return "image/svg+xml";
            }
        }
    }
}