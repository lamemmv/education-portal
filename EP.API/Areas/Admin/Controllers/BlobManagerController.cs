using EP.Data.Entities.Blobs;
using EP.Services;
using EP.Services.Blobs;
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

            if (entity != null)
            {
                string physicalPath = Path.Combine(_serverUploadPath, entity.FileName);
                Stream fileStream = new FileStream(physicalPath, FileMode.Open);

                return File(fileStream, entity.ContentType);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Post(IFormFile file)
        {
            if (!IsMultipartContentType())
            {
                return new StatusCodeResult((int)HttpStatusCode.UnsupportedMediaType);
            }

            if (file != null && file.Length > 0)
            {
                CreateServerUploadPathDirectory(_serverUploadPath);

                var entity = ToBlob(file);

                var content = await _blobService.UploadFileAsync(file.OpenReadStream(), entity.PhysicalPath);
                await _blobService.CreateAsync(entity);

                return Created(nameof(Post), new { entity.Id, content });
            }

            ModelState.AddModelError(string.Empty, "File should not be empty.");

            return BadRequest(ModelState);
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

        private static void CreateServerUploadPathDirectory(string phisicalPath)
        {
            if (!Directory.Exists(phisicalPath))
            {
                Directory.CreateDirectory(phisicalPath);
            }
        }

        private bool IsMultipartContentType()
        {
            string contentType = HttpContext.Request.ContentType;

            return !string.IsNullOrEmpty(contentType) &&
                contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private Blob ToBlob(IFormFile file)
        {
            string fileName = file.FileName; //ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            string newFileName = _blobService.GetRandomFileName(fileName);

            return new Blob
            {
                FileName = newFileName,
                ContentType = file.ContentType,
                PhysicalPath = Path.Combine(_serverUploadPath, newFileName),
                CreatedOnUtc = DateTime.UtcNow
            };
        }
    }
}
