using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace EP.API
{
    public static class StartupStaticFiles
    {
        private const string ImageDirectoryName = "image";

        public static IApplicationBuilder UseCustomStaticFiles(
            this IApplicationBuilder app,
            string webRootPath,
            string serverUploadFolder)
        {
            string directoryPath = EnsureAvailableDirectory(webRootPath, serverUploadFolder);

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(directoryPath),
                RequestPath = new PathString($"/{serverUploadFolder}/{ImageDirectoryName}")
            });

            return app;
        }

        private static string EnsureAvailableDirectory(string webRootPath, string serverUploadFolder)
        {
            string directoryPath = Path.Combine(webRootPath, serverUploadFolder, ImageDirectoryName);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            return directoryPath;
        }
    }
}