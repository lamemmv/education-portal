using EP.API.Areas.Admin.ViewModels.Blobs;
using EP.API.Extensions;
using EP.API.Filters;
using EP.Data.Entities.Blobs;
using EP.Data.Paginations;
using EP.Services.Blobs;
using EP.Services.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace EP.API.Areas.Admin.Controllers
{
    public class BlobManagerController : AdminController
    {
        private readonly IBlobService _blobService;

        public BlobManagerController(IBlobService blobService)
        {
            _blobService = blobService;
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
                return BadRequest(ApiResponse.BadRequest(nameof(id), $"The {id} is not a file."));
            }

            Stream fileStream = new FileStream(entity.PhysicalPath, FileMode.Open);

            return File(fileStream, entity.ContentType);
        }

        [HttpPost("Directory"), ValidateViewModel]
        public async Task<IActionResult> PostDirectory([FromBody]DirectoryViewModel viewModel)
        {
            var response = await _blobService.CreateDirectoryAsync(viewModel.Parent, viewModel.Name);

            return response.ToActionResult();
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
            var response = await _blobService.UpdateDirectoryAsync(id, viewModel.Parent, viewModel.Name);

            return response.ToActionResult();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _blobService.DeleteAsync(id);

            return response.ToActionResult();
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
