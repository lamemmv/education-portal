using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Linq;

namespace EP.API.Extensions
{
    public static class FormFileExtensions
    {
        public static bool IsAcceptableContentType(this IFormFile formFile)
        {
            var acceptableTypes = new[]
            {
                "image/gif",
                "image/png",
                "image/jpeg",
                "application/octet-stream",
                "application/pdf"
            };

            var contentType = formFile.ContentType.ToLowerInvariant();

            return acceptableTypes.Contains(contentType);
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
    }
}
