using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace EP.Services.Extensions
{
    public static class FormFileExtensions
    {
        public static string GetSubTypeFromContentType(this IFormFile formFile)
        {
            string contentTypeLower = formFile.ContentType ?? formFile.ContentType.ToLowerInvariant();

            switch (contentTypeLower)
            {
                case "image/gif":
                case "image/png":
                case "image/jpeg":
                case "application/zip":
                case "application/pdf":
                case "application/msword":
                    return GetSubType(contentTypeLower);

                case "application/x-rar-compressed":
                    return "rar";

                case "application/x-7z-compressed":
                    return "7z";

                case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
                    return "msword";

                case "application/vnd.ms-excel":
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":
                    return "msexcel";

                case "application/vnd.ms-powerpoint":
                case "application/vnd.openxmlformats-officedocument.presentationml.presentation":
                case "application/vnd.openxmlformats-officedocument.presentationml.slideshow":
                    return "mspowerpoint";

                default:
                    return null;
            }
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

        private static string GetSubType(string contentType)
            => contentType.Substring(contentType.IndexOf('/') + 1);
    }
}
