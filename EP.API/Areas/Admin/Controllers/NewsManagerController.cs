using EP.API.Areas.Admin.ViewModels;
using EP.API.Areas.Admin.ViewModels.News;
using EP.API.Extensions;
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
using System;
using System.Threading.Tasks;

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

        [HttpGet]
        public async Task<IPagedList<NewsItem>> Get([FromQuery]PaginationSearchViewModel viewModel)
        {
            return await _newsService.GetPagedListAsync(viewModel.Page, viewModel.Size);
        }

        [HttpGet("{id}")]
        public async Task<NewsItem> Get(string id)
        {
            return await _newsService.GetByIdAsync(id);
        }

        [HttpPost, ValidateViewModel]
        public async Task<IActionResult> Post([FromBody]NewsViewModel viewModel)
        {
            var entity = viewModel.Map<NewsViewModel, NewsItem>();
            entity.Blob = await GetEmbeddedBlobAsync(viewModel.BlobId);
            entity.CreatedOn = DateTime.UtcNow;

            var response = await _newsService.CreateAsync(entity);

            return response.ToActionResult();
        }

        [HttpPut("{id}"), ValidateViewModel]
        public async Task<IActionResult> Put(string id, [FromBody]NewsViewModel viewModel)
        {
            var entity = viewModel.Map<NewsViewModel, NewsItem>();
            entity.Id = id;
            entity.Blob = await GetEmbeddedBlobAsync(viewModel.BlobId);

            var response = await _newsService.UpdateAsync(entity);

            return response.ToActionResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _newsService.DeleteAsync(id);

            return response.ToActionResult();
        }

        private async Task<EmbeddedBlob> GetEmbeddedBlobAsync(string id)
        {
            return id.IsInvalidObjectId() ?
                null :
                await _blobService.GetEmbeddedBlobByIdAsync(id);
        }
    }
}
