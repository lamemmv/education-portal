using EP.API.Areas.Admin.ViewModels.Blobs;
using EP.API.Filters;
using EP.API.ViewModels.Errors;
using EP.Data.Entities.Blobs;
using EP.Data.Paginations;
using EP.Services.Blobs;
using EP.Services.Constants;
using EP.Services.Logs;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using System;

namespace EP.API.Areas.Admin.Controllers
{
    public class BlobManagerController : AdminController
    {
        private readonly IBlobService _blobService;
        private readonly IActivityLogService _activityLogService;

        public BlobManagerController(
            IBlobService blobService,
            IActivityLogService activityLogService)
        {
            _blobService = blobService;
            _activityLogService = activityLogService;
        }

        [HttpGet("ChildList")]
        public async Task<IPagedList<Blob>> ChildList(BlobSearchViewModel viewModel)
        {
            return await _blobService.GetChildListAsync(viewModel.Id, viewModel.Page, viewModel.Size);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var entity = await _blobService.GetByIdAsync(id);

            if (!_blobService.IsFile(entity))
            {
                ModelState.AddModelError(nameof(id), $"The {id} is not a file.");

                return BadRequest(new ApiError(ModelState));
            }

            Stream fileStream = new FileStream(entity.PhysicalPath, FileMode.Open);

            return File(fileStream, entity.ContentType);
        }

        [HttpPost("Directory"), ValidateViewModel]
        public async Task<IActionResult> PostDirectory([FromBody]DirectoryViewModel viewModel)
        {
            var parentEntity = await _blobService.GetByIdAsync(viewModel.Parent);

            if (parentEntity == null)
            {
                ModelState.AddModelError(nameof(viewModel.Parent), $"The {viewModel.Parent} is invalid.");

                return BadRequest(new ApiError(ModelState));
            }

            string directoryName = viewModel.Name.Trim();

            if (await _blobService.IsExistence(viewModel.Parent, directoryName))
            {
                ModelState.AddModelError(nameof(viewModel.Name), $"{directoryName} is existed.");

                return BadRequest(new ApiError(ModelState));
            }

            var entity = new Blob
            {
                Name = directoryName,
                Parent = viewModel.Parent,
                CreatedOn = DateTime.UtcNow
            };

            await _blobService.CreateAsync(entity);

            return Created(nameof(Directory), entity.Id);
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

        [HttpPut("Directory/{id}"), ValidateViewModel]
        public async Task<IActionResult> PutDirectory(string id, [FromBody]DirectoryViewModel viewModel)
        {
            var parentEntity = await _blobService.GetByIdAsync(viewModel.Parent);

            if (parentEntity == null)
            {
                ModelState.AddModelError(nameof(viewModel.Parent), $"The {viewModel.Parent} is invalid.");

                return BadRequest(new ApiError(ModelState));
            }

            string directoryName = viewModel.Name.Trim();

            if (await _blobService.IsExistence(viewModel.Parent, directoryName))
            {
                ModelState.AddModelError(nameof(viewModel.Name), $"{directoryName} is existed.");

                return BadRequest(new ApiError(ModelState));
            }

            var entity = new Blob
            {
                Id = id,
                Name = directoryName,
                Parent = viewModel.Parent
            };

            await _blobService.UpdateAsync(entity);

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            if (await _blobService.HasChildren(id))
            {
                ModelState.AddModelError(nameof(id), $"The {id} has sub directories or files.");

                return BadRequest(new ApiError(ModelState));
            }

            var entity = await _blobService.DeleteAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            if (_blobService.IsFile(entity) &&
                System.IO.File.Exists(entity.PhysicalPath))
            {
                System.IO.File.Delete(entity.PhysicalPath);
            }

            var activityLog = GetDeletedActivityLog(entity.GetType(), entity);
            await _activityLogService.CreateAsync(SystemKeyword.DeleteBlob, activityLog);

            return NoContent();
        }

        // private Blob BuildBlob(IFormFile file, int randomSize = 7)
        // {
        //     string contentType = file.ContentType;
        //     string firstMimeType = GetFirstMimeType(contentType);
        //     string publicBlobPath = GetPublicBlobPath(_webRootPath, _publicBlob, firstMimeType);

        //     string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.ToString().Trim('"');
        //     string name = Path.GetFileNameWithoutExtension(fileName);
        //     string extension = Path.GetExtension(fileName);
        //     string newFileName = $"{name}_{RandomUtils.Numberic(randomSize)}{extension}";

        //     return new Blob
        //     {
        //         Name = newFileName,
        //         FileExtension = extension.ToLowerInvariant(),
        //         ContentType = contentType,
        //         VirtualPath = string.IsNullOrWhiteSpace(firstMimeType) ?
        //             $"{_publicBlob}/{newFileName}" :
        //             $"{_publicBlob}/{firstMimeType}/{newFileName}",
        //         PhysicalPath = Path.Combine(publicBlobPath, newFileName),
        //         CreatedOn = DateTime.UtcNow
        //     };
        // }

        // private static string GetFirstMimeType(string contentType)
        // {
        //     if (string.IsNullOrWhiteSpace(contentType))
        //     {
        //         return null;
        //     }

        //     var types = contentType.Split('/', StringSplitOptions.RemoveEmptyEntries);

        //     if (types.Length == 0 || string.IsNullOrWhiteSpace(types[0]))
        //     {
        //         return null;
        //     }

        //     return types[0];
        // }

        // private static string GetPublicBlobPath(string webRootPath, string publicBlob, string firstMimeType)
        // {
        //     string publicBlobPath = string.IsNullOrWhiteSpace(firstMimeType) ?
        //         Path.Combine(webRootPath, publicBlob) :
        //         Path.Combine(webRootPath, publicBlob, firstMimeType);

        //     if (!Directory.Exists(publicBlobPath))
        //     {
        //         Directory.CreateDirectory(publicBlobPath);
        //     }

        //     return publicBlobPath;
        // }
    }
}
