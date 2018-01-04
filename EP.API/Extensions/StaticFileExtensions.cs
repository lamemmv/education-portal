using Microsoft.AspNetCore.Builder;
using System.IO;

namespace EP.API.Extensions
{
    public static class StaticFileExtensions
    {
        public static IApplicationBuilder UseCustomStaticFiles(
            this IApplicationBuilder app,
            string webRootPath,
            string publicBlob,
            string privateBlob)
        {
            return app
                .EnsureAvailableDirectories(webRootPath, publicBlob, privateBlob)
                .UseStaticFiles();
        }

        private static IApplicationBuilder EnsureAvailableDirectories(
            this IApplicationBuilder app,
            string webRootPath,
            string publicBlob,
            string privateBlob)
        {
            string publicBlobPath = Path.Combine(webRootPath, publicBlob);
            string privateBlobPath = Path.Combine(Directory.GetCurrentDirectory(), privateBlob);

            if (!Directory.Exists(publicBlobPath))
            {
                Directory.CreateDirectory(publicBlobPath);
            }

            if (!Directory.Exists(privateBlobPath))
            {
                Directory.CreateDirectory(privateBlobPath);
            }

            return app.InitDefaultBlob(publicBlob, publicBlobPath, privateBlob, privateBlobPath);
        }
    }
}