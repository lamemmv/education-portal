using EP.API.Areas.Admin.ViewModels.Blobs;
using EP.API.Filters;
using EP.Data.Entities.Blobs;
using EP.Services.Blobs;
using EP.Services.Extensions;
using ExpressMapper.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using System;

namespace EP.API.Areas.Admin.Controllers
{
    public class BlobManagerController : AdminController
    {
        private const string BlobFunctionName = Services.Constants.FunctionName.BlobManagement;
        
        private readonly IBlobService _blobService;

        public BlobManagerController(
            IHttpContextAccessor accessor,
            IAuthorizationService authorizationService,
            IBlobService blobService) : base(accessor, authorizationService)
        {
            _blobService = blobService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(BlobSearchViewModel viewModel)
        {
            await AuthorizeReadAsync(BlobFunctionName);

            var blob = await _blobService.GetBlobForChildListAsync(viewModel.Id);
            var childList = await _blobService.GetChildListAsync(viewModel.Id, viewModel.Page, viewModel.Size);

            return Ok(new { blob, childList });
        }

        [HttpGet("File/{id}")]
        public async Task<IActionResult> Download(string id)
        {
            await AuthorizeReadAsync(BlobFunctionName);

            var entity = await _blobService.GetByIdAsync(id);

            if (!entity.IsFile())
            {
                ModelState.AddModelError(nameof(id), $"The {id} is not a file.");

                return BadRequest(ModelState);
            }

            Stream stream = new FileStream(entity.PhysicalPath, FileMode.Open, FileAccess.Read);

            return File(stream, entity.ContentType, entity.Name);
        }

        [HttpPost("Folder"), ValidateViewModel]
        public async Task<IActionResult> PostFolder([FromBody]FolderViewModel viewModel)
        {
            await AuthorizeHostAsync(BlobFunctionName);

            var entity = viewModel.Map<FolderViewModel, Blob>();
            entity.CreatedOn = DateTime.UtcNow;

            await _blobService.CreateFolderAsync(entity, GetEmbeddedUser(), GetClientIP());

            return Created(string.Empty, entity.Id);
        }

        [HttpPost("File"), ValidateViewModel, ValidateMimeMultipartContent]
        public async Task<IActionResult> PostFile([FromForm]FileViewModel viewModel)
        {
            await AuthorizeUploadAsync(BlobFunctionName);

            if (viewModel.Files == null || viewModel.Files.Length == 0)
            {
                ModelState.AddModelError(nameof(viewModel.Files), "Files should not be empty.");

                return BadRequest(ModelState);
            }

            var ids = await _blobService.CreateFileAsync(
                viewModel.Files,
                viewModel.Parent.TrimNull(),
                GetEmbeddedUser(),
                GetClientIP());

            return Created(string.Empty, ids);
        }

        [HttpPut("Folder/{id}"), ValidateViewModel]
        public async Task<IActionResult> PutFolder(string id, [FromBody]FolderViewModel viewModel)
        {
            await AuthorizeHostAsync(BlobFunctionName);

            var entity = viewModel.Map<FolderViewModel, Blob>();
            entity.Id = id;

            var result = await _blobService.UpdateFolderAsync(entity, GetEmbeddedUser(), GetClientIP());

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string[] ids)
        {
            await AuthorizeHostAsync(BlobFunctionName);

            if (ids == null || ids.Length == 0)
            {
                ModelState.AddModelError(nameof(ids), "Ids should not be empty.");

                return BadRequest(ModelState);
            }

            await _blobService.DeleteAsync(ids, GetEmbeddedUser(), GetClientIP());

            return NoContent();
        }
    }
}
