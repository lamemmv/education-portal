using Microsoft.AspNetCore.Builder;
using System.IO;

namespace EP.API.Extensions
{
    public static class StaticFileApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCustomStaticFiles(
            this IApplicationBuilder app,
            string webRootPath,
            string publicBlob,
            string privateBlob)
        {
            EnsureAvailableDirectories(webRootPath, publicBlob, privateBlob);

            return app.UseStaticFiles();
        }

        private static void EnsureAvailableDirectories(string webRootPath, string publicBlob, string privateBlob)
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
        }
    }
}