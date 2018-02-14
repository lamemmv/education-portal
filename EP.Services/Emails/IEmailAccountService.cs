using EP.Data.Entities.Emails;
using EP.Data.Entities;
using EP.Data.Paginations;
using System.Threading.Tasks;

namespace EP.Services.Emails
{
    public interface IEmailAccountService
    {
        Task<IPagedList<EmailAccount>> GetPagedListAsync(int? page, int? size);

        Task<EmailAccount> GetByIdAsync(string id);

        Task<EmailAccount> CreateAsync(EmailAccount entity, EmbeddedUser embeddedUser, string ip);

        Task<bool> UpdateAsync(EmailAccount entity, EmbeddedUser embeddedUser, string ip);

        Task<bool> DeleteAsync(string id, EmbeddedUser embeddedUser, string ip);
    }
}
