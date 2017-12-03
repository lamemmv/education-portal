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

            CreateServerUploadPathDirectory(_serverUploadPath);

            string newFileName = GenerateNewFileName(file.ContentDisposition);

            var entity = new Blob
            {
                FileName = newFileName,
                ContentType = file.ContentType,
                PhysicalPath = Path.Combine(_serverUploadPath, newFileName),
                CreatedOnUtc = DateTime.UtcNow
            };

            entity = await _blobService.CreateAsync(entity);
            var content = await file.SaveAsAsync(entity.PhysicalPath);

            return Created(nameof(Post), new { entity.Id, content });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var entity = await _blobService.DeleteAsync(id);

            if (!string.IsNullOrEmpty(entity?.PhysicalPath) && System.IO.File.Exists(entity.PhysicalPath))
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

        private static void CreateServerUploadPathDirectory(string phisicalPath)
        {
            if (!Directory.Exists(phisicalPath))
            {
                Directory.CreateDirectory(phisicalPath);
            }
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
