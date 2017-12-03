using EP.API.Areas.Admin.ViewModels.News;
using EP.Data.Entities.News;
using EP.Data.Paginations;
using EP.Services.News;
using ExpressMapper.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EP.API.Areas.Admin.Controllers
{
    [Route("api/admin/[controller]")]
    public class NewsManagerController : Controller
    {
        private readonly INewsService _newsService;

        public NewsManagerController(INewsService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet]
        public async Task<IPagedList<NewsItem>> Get(int page, int size)
        {
            return await _newsService.FindAsync(page, size);
        }

        [HttpGet("{id}")]
        public async Task<NewsItem> Get(string id)
        {
            return await _newsService.FindAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]NewsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var entity = viewModel.Map<NewsViewModel, NewsItem>();
                entity.CreatedOnUtc = DateTime.UtcNow;

                await _newsService.CreateAsync(entity);

                return Created(nameof(Post), entity);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]NewsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var entity = viewModel.Map<NewsViewModel, NewsItem>();

                var result = await _newsService.UpdateAsync(id, entity);

                if (!result)
                {
                    return NotFound();
                }

                return NoContent();
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _newsService.DeleteAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
