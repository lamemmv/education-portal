using EP.Data.Entities.Emails;
using EP.Data.Paginations;
using System.Threading.Tasks;

namespace EP.Services.Emails
{
    public interface IEmailAccountService
    {
        Task<IPagedList<EmailAccount>> GetPagedListAsync(int? page, int? size);

        Task<EmailAccount> GetByIdAsync(string id);

        Task<EmailAccount> CreateAsync(EmailAccount entity);

        Task<EmailAccount> UpdateAsync(EmailAccount entity);

        Task<EmailAccount> DeleteAsync(string id);
    }
}
