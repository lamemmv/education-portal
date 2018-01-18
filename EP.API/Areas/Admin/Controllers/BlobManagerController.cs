using EP.API.Areas.Admin.ViewModels.Blobs;
using EP.API.Extensions;
using EP.API.Filters;
using EP.Data.Entities.Blobs;
using EP.Services.Blobs;
using EP.Services.Extensions;
using ExpressMapper.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EP.API.Areas.Admin.Controllers
{
    public class BlobManagerController : AdminController
    {
        private readonly IBlobService _blobService;

        public BlobManagerController(
            IBlobService blobService,
            IAuthorizationService authorizationService) : base(authorizationService)
        {
            _blobService = blobService;
        }

        protected override string FunctionName => Services.Constants.FunctionName.BlobManagement;

        [HttpGet]
        public async Task<IActionResult> Get(BlobSearchViewModel viewModel)
        {
            await AuthorizeReadAsync();

            var blob = await _blobService.GetBlobForChildListAsync(viewModel.Id);
            var childList = await _blobService.GetChildListAsync(viewModel.Id, viewModel.Page, viewModel.Size);

            return Ok(new { blob, childList });
        }

        [HttpGet("File/{id}")]
        public async Task<IActionResult> Download(string id)
        {
            await AuthorizeReadAsync();

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
            await AuthorizeHostAsync();

            var entity = viewModel.Map<FolderViewModel, Blob>();
            entity.CreatedOn = DateTime.UtcNow;

            var response = await _blobService.CreateFolderAsync(entity);

            return response.ToActionResult();
        }

        [HttpPost("File"), ValidateViewModel, ValidateMimeMultipartContent]
        public async Task<IActionResult> PostFile([FromForm]FileViewModel viewModel)
        {
            await AuthorizeUploadAsync();

            if (viewModel.Files == null || viewModel.Files.Length == 0)
            {
                ModelState.AddModelError(nameof(viewModel.Files), "Files should not be empty.");

                return BadRequest(ModelState);
            }

            var results = await _blobService.CreateFileAsync(viewModel.Files, viewModel.Parent.TrimNull());

            return Ok(results);
        }

        [HttpPut("Folder/{id}"), ValidateViewModel]
        public async Task<IActionResult> PutFolder(string id, [FromBody]FolderViewModel viewModel)
        {
            await AuthorizeHostAsync();

            var entity = viewModel.Map<FolderViewModel, Blob>();
            entity.Id = id;

            var response = await _blobService.UpdateFolderAsync(entity);

            return response.ToActionResult();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string[] ids)
        {
            await AuthorizeHostAsync();

            if (ids == null || ids.Length == 0)
            {
                ModelState.AddModelError(nameof(ids), "Ids should not be empty.");

                return BadRequest(ModelState);
            }

            var results = await _blobService.DeleteAsync(ids);

            return Ok(results);
        }
    }
}
