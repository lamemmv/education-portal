using EP.API.Areas.Admin.ViewModels.Blobs;
using EP.API.Filters;
using EP.API.ViewModels.Errors;
using EP.Data.Entities.Blobs;
using EP.Services;
using EP.Services.Blobs;
using EP.Services.Constants;
using EP.Services.Logs;
using EP.Services.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

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

        [HttpGet("ChildList")]
        public async Task<IEnumerable<Blob>> ChildList(string id)
        {
            return await _blobService.GetChildListAsync(id);
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

        [HttpPost("Directory"), ValidateViewModel]
        public async Task<IActionResult> PostDirectory([FromBody]DirectoryViewModel viewModel)
        {
            string parentPhysicalPath = await _blobService.GetPhysicalPath(viewModel.Parent);

            if (string.IsNullOrEmpty(parentPhysicalPath))
            {
                var parentName = nameof(viewModel.Parent);
                ModelState.AddModelError(parentName, $"The {parentName} field is invalid.");

                return BadRequest(new ApiError(ModelState));
            }

            string directoryName = viewModel.Name.Trim();
            var blob = new Blob
            {
                Name = directoryName,
                PhysicalPath = Path.Combine(parentPhysicalPath, directoryName),
                Parent = viewModel.Parent,
                CreatedOn = DateTime.UtcNow
            };

            if (await _blobService.ExistBlob(viewModel.Parent, directoryName) ||
                Directory.Exists(blob.PhysicalPath))
            {
                ModelState.AddModelError(nameof(viewModel.Name), $"{directoryName} is existed.");

                return BadRequest(new ApiError(ModelState));
            }

            Directory.CreateDirectory(blob.PhysicalPath);
            await _blobService.CreateAsync(blob);

            return Created(nameof(Directory), blob.Id);

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

        //[HttpPost("File"), ValidateViewModel, ValidateMimeMultipartContent]
        //public async Task<IActionResult> PostFile([FromForm]FileViewModel viewModel)
        //{
        //    if (viewModel.Files == null || files.Length == 0)
        //    {
        //        ModelState.AddModelError(nameof(files), "Files should not be empty.");

        //        return BadRequest(new ApiError(ModelState));
        //    }

        //    Blob entity;
        //    IList<string> ids = new List<string>();

        //    foreach (var file in files)
        //    {
        //        entity = BuildBlob(file);
        //        var activityLog = GetCreatedActivityLog(entity.GetType(), entity);

        //        await Task.WhenAll(
        //            _blobService.CreateAsync(entity),
        //            file.SaveAsAsync(entity.PhysicalPath),
        //            _activityLogService.CreateAsync(SystemKeyword.CreateBlob, activityLog));

        //        ids.Add(entity.Id);
        //    }

        //    return Created(nameof(Post), ids);
        //}

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
            string firstMimeType = GetFirstMimeType(contentType);
            string publicBlobPath = GetPublicBlobPath(_webRootPath, _publicBlob, firstMimeType);

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
