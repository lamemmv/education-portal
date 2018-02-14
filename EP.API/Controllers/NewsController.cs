using EP.Data.Entities.News;
using EP.Data.Paginations;
using EP.Services.News;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EP.API.Controllers
{
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet]
        public async Task<IPagedList<NewsItem>> Get(int? page, int? size)
            => await _newsService.GetPagedListAsync(page, size);

        [HttpGet("{id}")]
        public async Task<NewsItem> Get(string id)
            => await _newsService.GetByIdAsync(id);
    }
}
