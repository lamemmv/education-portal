using EP.Data.Entities.Emails;
using EP.Data.Paginations;
using EP.Services.Models;
using System.Threading.Tasks;

namespace EP.Services.Emails
{
    public interface IEmailAccountService
    {
        Task<IPagedList<EmailAccount>> GetPagedListAsync(int? page, int? size);

        Task<EmailAccount> GetByIdAsync(string id);

        Task<ApiServerResult> CreateAsync(EmailAccount entity);

        Task<ApiServerResult> UpdateAsync(EmailAccount entity);

        Task<ApiServerResult> DeleteAsync(string id);
    }
}
