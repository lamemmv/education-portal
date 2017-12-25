using EP.API.Areas.Admin.ViewModels.Blobs;
using EP.API.Extensions;
using EP.API.Filters;
using EP.API.ViewModels.Errors;
using EP.Data.Constants;
using EP.Data.Entities.Blobs;
using EP.Data.Paginations;
using EP.Services.Blobs;
using EP.Services.Logs;
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
        private readonly IActivityLogService _activityLogService;
        private readonly string _serverUploadFolder;
        private readonly string _webRootPath;

        public BlobManagerController(
            IBlobService blobService,
            IActivityLogService activityLogService,
            IOptionsSnapshot<AppSettings> options,
            IHostingEnvironment hostingEnvironment)
        {
            _blobService = blobService;
            _activityLogService = activityLogService;
            _serverUploadFolder = options.Value.ServerUploadFolder;
            _webRootPath = hostingEnvironment.WebRootPath;
        }

        [HttpGet]
        public async Task<IPagedList<Blob>> Get(BlobSearchViewModel viewModel)
        {
            return await _blobService.GetPagedListAsync(viewModel.Ext, viewModel.Page, viewModel.Size);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var entity = await _blobService.GetByIdAsync(id);

            if (entity == null || !System.IO.File.Exists(entity.PhysicalPath))
            {
                return NotFound();
            }

            Stream fileStream = new FileStream(entity.PhysicalPath, FileMode.Open);

            return File(fileStream, entity.ContentType);
        }

        [HttpPost, ValidateMimeMultipartContent]
        public async Task<IActionResult> Post(IFormFile[] files)
        {
            if (files == null || files.Length == 0)
            {
                ModelState.AddModelError(nameof(files), "Files should not be empty.");

                return BadRequest(new ApiError(ModelState));
            }

            Blob entity;
            IList<string> ids = new List<string>();

            foreach (var file in files)
            {
                entity = BuildBlob(file);
                var activityLog = GetCreatedActivityLog(entity.GetType(), entity);

                await Task.WhenAll(
                    _blobService.CreateAsync(entity),
                    file.SaveAsAsync(entity.PhysicalPath),
                    _activityLogService.CreateAsync(SystemKeyword.CreateBlob, activityLog));

                ids.Add(entity.Id);
            }

            return Created(nameof(Post), ids);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var entity = await _blobService.DeleteAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            if (System.IO.File.Exists(entity.PhysicalPath))
            {
                System.IO.File.Delete(entity.PhysicalPath);
            }

            var activityLog = GetDeletedActivityLog(entity.GetType(), entity);
            await _activityLogService.CreateAsync(SystemKeyword.DeleteBlob, activityLog);

            return NoContent();
        }

        private Blob BuildBlob(IFormFile file, int randomSize = 7)
        {
            string contentType = file.ContentType;
            string physicalPath = GetServerUploadPathDirectory(contentType);

            string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.ToString().Trim('"');
            string name = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);
            string newFileName = $"{name}_{RandomUtils.Numberic(randomSize)}{extension}";

            return new Blob
            {
                FileName = newFileName,
                FileExtension = extension.ToLowerInvariant(),
                ContentType = contentType,
                VirtualPath = $"{_serverUploadFolder}/{newFileName}",
                PhysicalPath = Path.Combine(physicalPath, newFileName),
                CreatedOn = DateTime.UtcNow
            };
        }

        private string GetServerUploadPathDirectory(string contentType)
        {
            string physicalPath = Path.Combine(_webRootPath, _serverUploadFolder);

            if (!string.IsNullOrWhiteSpace(contentType))
            {
                var types = contentType.Split('/', StringSplitOptions.RemoveEmptyEntries);

                if (types.Length > 0 && !string.IsNullOrWhiteSpace(types[0]))
                {
                    physicalPath = Path.Combine(physicalPath, types[0]);

                    CheckAndCreateDirectory(physicalPath);
                }
            }

            return physicalPath;
        }

        private static void CheckAndCreateDirectory(string physicalPath)
        {
            if (!Directory.Exists(physicalPath))
            {
                Directory.CreateDirectory(physicalPath);
            }
        }
    }
}
