using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EP.Services.Extensions
{
    public static class FormFileExtensions
    {
        public static string GetTypeFromContentType(this IFormFile formFile)
        {
            return GetAcceptableTypes(formFile.ContentType);
        }

        public async static Task SaveAsAsync(
            this IFormFile formFile,
            string physicalPath,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            const int DefaultBufferSize = 80 * 1024;

            using (Stream fileStream = new FileStream(physicalPath, FileMode.Create))
            {
                Stream inputStream = formFile.OpenReadStream();

                await inputStream.CopyToAsync(fileStream, DefaultBufferSize, cancellationToken);
            }
        }

        private static string GetAcceptableTypes(string contentType)
        {
            string contentTypeLower = contentType ?? contentType.ToLowerInvariant();

            switch (contentTypeLower)
            {
                case "image/gif":
                case "image/png":
                case "image/jpeg":
                    return "image";

                case "application/octet-stream":
                case "application/pdf":
                    return "application";

                default:
                    return null;
            }
        }
    }
}
