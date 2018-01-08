using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace EP.Services.Extensions
{
    public static class FormFileExtensions
    {
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
