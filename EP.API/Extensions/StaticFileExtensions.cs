using EP.Data.Entities.Blobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace EP.API.Extensions
{
    public static class StaticFileExtensions
    {
        public static IApplicationBuilder UseCustomStaticFiles(
            this IApplicationBuilder app,
            string webRootPath,
            string contentRootPath,
            IConfiguration configuration)
            => app
                .EnsureAvailableDirectories(webRootPath, contentRootPath, configuration)
                .UseStaticFiles();

        private static IApplicationBuilder EnsureAvailableDirectories(
            this IApplicationBuilder app,
            string webRootPath,
            string contentRootPath,
            IConfiguration configuration)
        {
            var publicFolderName = configuration["AppSettings:PublicFolder"];
            var publicFolderPath = Path.Combine(webRootPath, publicFolderName);
            CreateDirectoryIfNotExist(publicFolderPath);

            var privateFolderName = configuration["AppSettings:PrivateFolder"];
            var privateFolderPath = Path.Combine(contentRootPath, privateFolderName);
            CreateDirectoryIfNotExist(privateFolderPath);

            var commonFolderName = configuration["AppSettings:CommonFolder"];
            var commonFolderPath = Path.Combine(webRootPath, commonFolderName);
            CreateDirectoryIfNotExist(commonFolderPath);

            return app.InitDefaultBlob(new[]
            {
                BuildBlob(publicFolderName, publicFolderPath, publicFolderName),
                BuildBlob(privateFolderName, privateFolderPath),
                BuildBlob(commonFolderName, commonFolderPath, commonFolderName)
            });
        }

        private static void CreateDirectoryIfNotExist(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private static Blob BuildBlob(string name, string physicalPath, string virtualPath = null)
            => new Blob
            {
                Name = name,
                VirtualPath = virtualPath,
                PhysicalPath = physicalPath
            };
    }
}