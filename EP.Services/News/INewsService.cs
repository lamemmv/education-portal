using EP.Data.Entities.News;
using EP.Data.Paginations;
using System.Threading.Tasks;

namespace EP.Services.News
{
    public interface INewsService
    {
        Task<IPagedList<NewsItem>> GetPagedListAsync(int? page, int? size);

        Task<NewsItem> GetByIdAsync(string id);

        Task<NewsItem> CreateAsync(NewsItem entity);

        Task<bool> UpdateAsync(NewsItem entity);

        Task<bool> DeleteAsync(string id);
    }
}
