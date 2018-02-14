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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace EP.API.Areas.Admin.Controllers
{
    public class NewsManagerController : AdminController
    {
        private const string NewsFunctionName = Services.Constants.FunctionName.NewsManagement;
        
        private readonly INewsService _newsService;
        private readonly IBlobService _blobService;

        public NewsManagerController(
            IHttpContextAccessor accessor,
            IAuthorizationService authorizationService,
            INewsService newsService,
            IBlobService blobService) : base(accessor, authorizationService)
        {
            _newsService = newsService;
            _blobService = blobService;
        }

        [HttpGet]
        public async Task<IPagedList<NewsItem>> Get([FromQuery]PaginationSearchViewModel viewModel)
        {
            await AuthorizeReadAsync(NewsFunctionName);

            return await _newsService.GetPagedListAsync(viewModel.Page, viewModel.Size);
        }

        [HttpGet("{id}")]
        public async Task<NewsItem> Get(string id)
        {
            await AuthorizeReadAsync(NewsFunctionName);

            return await _newsService.GetByIdAsync(id);
        }

        [HttpPost, ValidateViewModel]
        public async Task<IActionResult> Post([FromBody]NewsViewModel viewModel)
        {
            await AuthorizeHostAsync(NewsFunctionName);

            var entity = viewModel.Map<NewsViewModel, NewsItem>();
            entity.Blob = await GetEmbeddedBlobAsync(viewModel.BlobId);
            entity.CreatedOn = DateTime.UtcNow;

            await _newsService.CreateAsync(entity, GetEmbeddedUser(), GetClientIP());

            return Created(string.Empty, entity.Id);
        }

        [HttpPut("{id}"), ValidateViewModel]
        public async Task<IActionResult> Put(string id, [FromBody]NewsViewModel viewModel)
        {
            await AuthorizeHostAsync(NewsFunctionName);

            var entity = viewModel.Map<NewsViewModel, NewsItem>();
            entity.Id = id;
            entity.Blob = await GetEmbeddedBlobAsync(viewModel.BlobId);

            var result = await _newsService.UpdateAsync(entity, GetEmbeddedUser(), GetClientIP());

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await AuthorizeHostAsync(NewsFunctionName);

            var result = await _newsService.DeleteAsync(id, GetEmbeddedUser(), GetClientIP());

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        private async Task<EmbeddedBlob> GetEmbeddedBlobAsync(string id)
            => id.IsInvalidObjectId() ?
                null :
                await _blobService.GetEmbeddedBlobByIdAsync(id);
    }
}
