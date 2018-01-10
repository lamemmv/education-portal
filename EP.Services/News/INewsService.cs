using EP.Data.Entities.News;
using EP.Data.Paginations;
using EP.Services.Models;
using System.Threading.Tasks;

namespace EP.Services.News
{
    public interface INewsService
    {
        Task<IPagedList<NewsItem>> GetPagedListAsync(int? page, int? size);

        Task<NewsItem> GetByIdAsync(string id);

        Task<ApiServerResult> CreateAsync(NewsItem entity);

        Task<ApiServerResult> UpdateAsync(NewsItem entity);

        Task<ApiServerResult> DeleteAsync(string id);
    }
}
