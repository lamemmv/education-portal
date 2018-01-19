using EP.API.Areas.Admin.ViewModels.News;
using EP.API.Areas.Admin.ViewModels;
using EP.API.Filters;
using EP.Data.Entities.Blobs;
using EP.Data.Entities.News;
using EP.Data.Extensions;
using EP.Data.Paginations;
using EP.Services.Blobs;
using EP.Services.News;
using ExpressMapper.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace EP.API.Areas.Admin.Controllers
{
    public class NewsManagerController : AdminController
    {
        private readonly INewsService _newsService;
        private readonly IBlobService _blobService;

        public NewsManagerController(
            INewsService newsService,
            IBlobService blobService,
            IAuthorizationService authorizationService) : base(authorizationService)
        {
            _newsService = newsService;
            _blobService = blobService;
        }

        protected override string FunctionName => Services.Constants.FunctionName.NewsManagement;

        [HttpGet]
        public async Task<IPagedList<NewsItem>> Get([FromQuery]PaginationSearchViewModel viewModel)
        {
            await AuthorizeReadAsync();

            return await _newsService.GetPagedListAsync(viewModel.Page, viewModel.Size);
        }

        [HttpGet("{id}")]
        public async Task<NewsItem> Get(string id)
        {
            await AuthorizeReadAsync();

            return await _newsService.GetByIdAsync(id);
        }

        [HttpPost, ValidateViewModel]
        public async Task<IActionResult> Post([FromBody]NewsViewModel viewModel)
        {
            await AuthorizeHostAsync();

            var entity = viewModel.Map<NewsViewModel, NewsItem>();
            entity.Blob = await GetEmbeddedBlobAsync(viewModel.BlobId);
            entity.CreatedOn = DateTime.UtcNow;

            await _newsService.CreateAsync(entity);

            return Created(string.Empty, entity.Id);
        }

        [HttpPut("{id}"), ValidateViewModel]
        public async Task<IActionResult> Put(string id, [FromBody]NewsViewModel viewModel)
        {
            await AuthorizeHostAsync();

            var entity = viewModel.Map<NewsViewModel, NewsItem>();
            entity.Id = id;
            entity.Blob = await GetEmbeddedBlobAsync(viewModel.BlobId);

            var result = await _newsService.UpdateAsync(entity);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await AuthorizeHostAsync();

            var result = await _newsService.DeleteAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        private async Task<EmbeddedBlob> GetEmbeddedBlobAsync(string id)
        {
            return id.IsInvalidObjectId() ?
                null :
                await _blobService.GetEmbeddedBlobByIdAsync(id);
        }
    }
}
