using EP.API.Areas.Admin.ViewModels.Blobs;
using EP.API.Extensions;
using EP.API.Filters;
using EP.API.ViewModels.Errors;
using EP.Data.Entities.Blobs;
using EP.Data.Paginations;
using EP.Services.Blobs;
using EP.Services.Constants;
using EP.Services.Logs;
using EP.Services.Utilities;
using EP.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
using EP.Data.Extensions;

namespace EP.API.Areas.Admin.Controllers
{
    public class BlobManagerController : AdminController
    {
        private readonly IBlobService _blobService;
        private readonly IActivityLogService _activityLogService;
        private readonly string _webRootPath;
        private readonly string _publicBlob;

        public BlobManagerController(
            IBlobService blobService,
            IActivityLogService activityLogService,
            IHostingEnvironment hostingEnvironment,
            AppSettings appSettings)
        {
            _blobService = blobService;
            _activityLogService = activityLogService;
            _webRootPath = hostingEnvironment.WebRootPath;
            _publicBlob = appSettings.PublicBlob;
        }

        [HttpGet]
        public async Task<IPagedList<Blob>> Get([FromQuery]BlobSearchViewModel viewModel)
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
        public async Task<IActionResult> Post([FromForm]BlobViewModel viewModel)
        {
            var parent = await _blobService.GetByIdAsync(viewModel.Parent);

            if (parent == null)
            {
                var parentName = nameof(viewModel.Parent);
                ModelState.AddModelError(parentName, $"{parentName} is invalid.");

                return BadRequest(new ApiError(ModelState));
            }

            if (viewModel.IsValidDirectory)
            {

            }

            // if (files == null || files.Length == 0)
            // {
            //     ModelState.AddModelError(nameof(files), "Files should not be empty.");

            //     return BadRequest(new ApiError(ModelState));
            // }

            // Blob entity;
            // IList<string> ids = new List<string>();

            // foreach (var file in files)
            // {
            //     entity = BuildBlob(file);
            //     var activityLog = GetCreatedActivityLog(entity.GetType(), entity);

            //     await Task.WhenAll(
            //         _blobService.CreateAsync(entity),
            //         file.SaveAsAsync(entity.PhysicalPath),
            //         _activityLogService.CreateAsync(SystemKeyword.CreateBlob, activityLog));

            //     ids.Add(entity.Id);
            // }

            // return Created(nameof(Post), ids);
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

        // private async Task<bool> IsValidParentBlob(string parent)
        // {
        //     return parent.IsInvalidObjectId() ?
        //         false :
        //         await _blobService.GetBlobByIdAsync(parent);
        // }

        private Blob BuildBlob(IFormFile file, int randomSize = 7)
        {
            string contentType = file.ContentType;
            string firstMimeType = GetFirstMimeType(contentType);
            string publicBlobPath = GetPublicBlobPath(_webRootPath ,_publicBlob, firstMimeType);

            string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.ToString().Trim('"');
            string name = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);
            string newFileName = $"{name}_{RandomUtils.Numberic(randomSize)}{extension}";

            return new Blob
            {
                Name = newFileName,
                FileExtension = extension.ToLowerInvariant(),
                ContentType = contentType,
                VirtualPath = string.IsNullOrWhiteSpace(firstMimeType) ?
                    $"{_publicBlob}/{newFileName}" :
                    $"{_publicBlob}/{firstMimeType}/{newFileName}",
                PhysicalPath = Path.Combine(publicBlobPath, newFileName),
                CreatedOn = DateTime.UtcNow
            };
        }

        private static string GetFirstMimeType(string contentType)
        {
            if (string.IsNullOrWhiteSpace(contentType))
            {
                return null;
            }

            var types = contentType.Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (types.Length == 0 || string.IsNullOrWhiteSpace(types[0]))
            {
                return null;
            }

            return types[0];
        }

        private static string GetPublicBlobPath(string webRootPath, string publicBlob, string firstMimeType)
        {
            string publicBlobPath = string.IsNullOrWhiteSpace(firstMimeType) ?
                Path.Combine(webRootPath, publicBlob) :
                Path.Combine(webRootPath, publicBlob, firstMimeType);

            if (!Directory.Exists(publicBlobPath))
            {
                Directory.CreateDirectory(publicBlobPath);
            }

            return publicBlobPath;
        }
    }
}
