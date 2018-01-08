using EP.API.Areas.Admin.ViewModels.Blobs;
using EP.API.Extensions;
using EP.API.Filters;
using EP.Data.Entities.Blobs;
using EP.Services.Blobs;
using ExpressMapper.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
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

        [HttpGet]
        public async Task<dynamic> Get(BlobSearchViewModel viewModel)
        {
            var blob = await _blobService.GetBlobForChildListAsync(viewModel.Id);
            var childList = await _blobService.GetChildListAsync(viewModel.Id, viewModel.Page, viewModel.Size);

            return new { blob, childList };
        }

        [HttpPost("Folder"), ValidateViewModel]
        public async Task<IActionResult> PostFolder([FromBody]FolderViewModel viewModel)
        {
            var entity = viewModel.Map<FolderViewModel, Blob>();
            entity.CreatedOn = DateTime.UtcNow;

            var response = await _blobService.CreateFolderAsync(entity);

            return response.ToActionResult();
        }

        [HttpPost("File"), ValidateViewModel, ValidateMimeMultipartContent]
        public async Task<IActionResult> PostFile([FromForm]FileViewModel viewModel)
        {
            if (viewModel.Files == null || viewModel.Files.Length == 0)
            {
                ModelState.AddModelError(nameof(viewModel.Files), "Files should not be empty.");

                return BadRequest(ModelState);
            }

            var response = await _blobService.CreateFileAsync(viewModel.Parent.Trim(), viewModel.Files);

            return Ok();//response.ToActionResult();
        }

        [HttpPut("Folder/{id}"), ValidateViewModel]
        public async Task<IActionResult> PutFolder(string id, [FromBody]FolderViewModel viewModel)
        {
            var entity = viewModel.Map<FolderViewModel, Blob>();
            entity.Id = id;

            var response = await _blobService.UpdateFolderAsync(entity);

            return response.ToActionResult();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _blobService.DeleteAsync(id);

            return response.ToActionResult();
        }
    }
}
