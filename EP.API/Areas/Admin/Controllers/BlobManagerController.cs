using EP.API.Extensions;
using EP.Data.Entities.Blobs;
using EP.Services;
using EP.Services.Blobs;
using EP.Services.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace EP.API.Areas.Admin.Controllers
{
    public class BlobManagerController : AdminController
    {
        private readonly IBlobService _blobService;
        private readonly string _serverUploadPath;

        public BlobManagerController(
            IBlobService blobService,
            IOptionsSnapshot<AppSettings> options,
            IHostingEnvironment hostingEnvironment)
        {
            _blobService = blobService;
            _serverUploadPath = Path.Combine(hostingEnvironment.WebRootPath, options.Value.ServerUploadFolder);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var entity = await _blobService.FindAsync(id);
            string physicalPath = entity?.PhysicalPath;

            if (string.IsNullOrEmpty(physicalPath) || !System.IO.File.Exists(physicalPath))
            {
                return NotFound();
            }

            Stream fileStream = new FileStream(physicalPath, FileMode.Open);

            return File(fileStream, entity.ContentType);
        }

        [HttpPost]
        public async Task<IActionResult> Post(IFormFile file)
        {
            if (!IsMultipartContentType(HttpContext.Request.ContentType))
            {
                return new StatusCodeResult((int)HttpStatusCode.UnsupportedMediaType);
            }

            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "File should not be empty.");

                return BadRequest(ModelState);
            }

            string contentType = file.ContentType;
            string physicalPath = CreateServerUploadPathDirectory(_serverUploadPath, contentType);
            string newFileName = GenerateNewFileName(file.ContentDisposition);

            var entity = new Blob
            {
                FileName = newFileName,
                ContentType = contentType,
                PhysicalPath = Path.Combine(physicalPath, newFileName),
                CreatedOnUtc = DateTime.UtcNow
            };


            await Task.WhenAll(
                _blobService.CreateAsync(entity),
                file.SaveAsAsync(entity.PhysicalPath));

            return Created(nameof(Post), entity.Id);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var entity = await _blobService.DeleteAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(entity.PhysicalPath) && System.IO.File.Exists(entity.PhysicalPath))
            {
                System.IO.File.Delete(entity.PhysicalPath);
            }

            return NoContent();
        }

        private static bool IsMultipartContentType(string contentType)
        {
            return !string.IsNullOrEmpty(contentType) &&
                contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static string CreateServerUploadPathDirectory(string physicalPath, string contentType)
        {
            if (!Directory.Exists(physicalPath))
            {
                Directory.CreateDirectory(physicalPath);
            }

            if (!string.IsNullOrWhiteSpace(contentType))
            {
                var types = contentType.Split('/', StringSplitOptions.RemoveEmptyEntries);

                if (types.Length > 0 && !string.IsNullOrWhiteSpace(types[0]))
                {
                    physicalPath = Path.Combine(physicalPath, types[0]);

                    if (!Directory.Exists(physicalPath))
                    {
                        Directory.CreateDirectory(physicalPath);
                    }
                }
            }

            return physicalPath;
        }

        private static string GenerateNewFileName(string contentDisposition, int randomSize = 7)
        {
            string fileName = ContentDispositionHeaderValue.Parse(contentDisposition).FileName.ToString().Trim('"');

            string name = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);

            return $"{name}_{RandomUtils.Numberic(randomSize)}{extension}";
        }
    }
}
