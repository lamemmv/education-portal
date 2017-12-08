using EP.API.Extensions;
using EP.API.Filters;
using EP.Data.Entities.Blobs;
using EP.Data.Paginations;
using EP.Services.Blobs;
using EP.Services.Utilities;
using EP.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;

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

        [HttpGet]
        public async Task<IPagedList<Blob>> Get(string ext, int? page, int? size)
        {
            return await _blobService.FindAsync(ext, page, size);
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

        [HttpPost, ValidateMimeMultipartContent]
        public async Task<IActionResult> Post(IFormFile[] files)
        {
            if (files == null || files.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "Files should not be empty.");

                return BadRequest(ModelState);
            }

            Blob entity;
            IList<string> ids = new List<string>();

            foreach (var file in files)
            {
                entity = BuildBlob(file);

                await Task.WhenAll(
                    _blobService.CreateAsync(entity),
                    file.SaveAsAsync(entity.PhysicalPath));

                ids.Add(entity.Id);
            }

            return Created(nameof(Post), ids);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var entity = await _blobService.DeleteAsync(id);
            string physicalPath = entity?.PhysicalPath;

            if (string.IsNullOrEmpty(physicalPath))
            {
                return NotFound();
            }

            if (System.IO.File.Exists(physicalPath))
            {
                System.IO.File.Delete(physicalPath);
            }

            return NoContent();
        }

        private Blob BuildBlob(IFormFile file, int randomSize = 7)
        {
            string contentType = file.ContentType;
            string physicalPath = _blobService.GetServerUploadPathDirectory(_serverUploadPath, contentType);

            string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.ToString().Trim('"');
            string name = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);
            string newFileName = $"{name}_{RandomUtils.Numberic(randomSize)}{extension}";

            var entity = new Blob
            {
                FileName = newFileName,
                FileExtension = extension,
                ContentType = contentType,
                PhysicalPath = Path.Combine(physicalPath, newFileName),
                CreatedOnUtc = DateTime.UtcNow
            };

            return entity;
        }
    }
}
