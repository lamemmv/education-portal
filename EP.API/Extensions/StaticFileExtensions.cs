using EP.Data.Entities.Blobs;
using EP.Services.Models;
using Microsoft.AspNetCore.Builder;
using System.IO;

namespace EP.API.Extensions
{
    public static class StaticFileExtensions
    {
        public static IApplicationBuilder UseCustomStaticFiles(
            this IApplicationBuilder app,
            string webRootPath,
            string contentRootPath,
            BlobFolders blobFolders)
            => app
                .EnsureAvailableDirectories(webRootPath, contentRootPath, blobFolders)
                .UseStaticFiles();

        private static IApplicationBuilder EnsureAvailableDirectories(
            this IApplicationBuilder app,
            string webRootPath,
            string contentRootPath,
            BlobFolders blobFolders)
        {
            var publicFolderPath = Path.Combine(webRootPath, blobFolders.Public);
            CreateDirectoryIfNotExist(publicFolderPath);

            var privateFolderPath = Path.Combine(contentRootPath, blobFolders.Private);
            CreateDirectoryIfNotExist(privateFolderPath);

            var commonFolderPath = Path.Combine(webRootPath, blobFolders.Common);
            CreateDirectoryIfNotExist(commonFolderPath);

            return app.InitDefaultBlob(new[]
            {
                BuildBlob(blobFolders.Public, publicFolderPath, blobFolders.Public),
                BuildBlob(blobFolders.Private, privateFolderPath),
                BuildBlob(blobFolders.Common, commonFolderPath, blobFolders.Common)
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