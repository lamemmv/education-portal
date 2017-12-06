using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace EP.API.Extensions
{
    public static class FormFileExtensions
    {
        private static int DefaultBufferSize = 80 * 1024;

        public async static Task<byte[]> SaveAsAsync(
            this IFormFile formFile,
            string physicalPath,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            using (Stream fileStream = new FileStream(physicalPath, FileMode.Create))
            {
                Stream inputStream = formFile.OpenReadStream();
                await inputStream.CopyToAsync(fileStream, DefaultBufferSize, cancellationToken);

                return ReadFile(inputStream);
            }
        }

        private static byte[] ReadFile(Stream inputStream)
        {
            inputStream.Position = 0;
            var binaryReader = new BinaryReader(inputStream);

            return binaryReader.ReadBytes((int)inputStream.Length);
        }
    }
}
