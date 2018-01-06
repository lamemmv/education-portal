using EP.API.Areas.Admin.ViewModels.Blobs;
using EP.API.Extensions;
using EP.API.Filters;
using EP.Data.Entities.Blobs;
using EP.Data.Paginations;
using EP.Services.Blobs;
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
                ModelState.AddModelError(nameof(id), $"The {id} is not a file.");

                return BadRequest(ModelState);
            }

            Stream fileStream = new FileStream(entity.PhysicalPath, FileMode.Open);

            return File(fileStream, entity.ContentType);
        }

        [HttpPost("Directory"), ValidateViewModel]
        public async Task<IActionResult> PostDirectory([FromBody]DirectoryViewModel viewModel)
        {
            var response = await _blobService.CreateDirectoryAsync(
                viewModel.Name.Trim(),
                viewModel.Parent.Trim());

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

            var response = await _blobService.CreateFileAsync(viewModel.Parent, viewModel.Files);

            return response.ToActionResult();
        }

        [HttpPut("Directory/{id}"), ValidateViewModel]
        public async Task<IActionResult> PutDirectory(string id, [FromBody]DirectoryViewModel viewModel)
        {
            var response = await _blobService.UpdateDirectoryAsync(
                id,
                viewModel.Name.Trim(),
                viewModel.Parent.Trim());

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
