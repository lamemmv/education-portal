using EP.Data.DbContext;
using EP.Data.Entities.Emails;
using EP.Data.Paginations;
using EP.Data.Repositories;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace EP.Services.Emails
{
    public sealed class EmailAccountService : IEmailAccountService
    {
        private readonly IRepository<EmailAccount> _emailAccounts;

        public EmailAccountService(MongoDbContext dbContext)
        {
            _emailAccounts = dbContext.EmailAccounts;
        }

        public async Task<IPagedList<EmailAccount>> FindAsync(int? page, int? size)
        {
            return await _emailAccounts.FindAsync(skip: page, take: size);
        }

        public async Task<EmailAccount> FindAsync(string id)
        {
            return await _emailAccounts.FindAsync(id);
        }

        public async Task<EmailAccount> CreateAsync(EmailAccount entity)
        {
            return await _emailAccounts.CreateAsync(entity);
        }

        public async Task<bool> UpdateAsync(string id, EmailAccount entity)
        {
            var update = Builders<EmailAccount>.Update
                .Set(e => e.Email, entity.Email)
                .Set(e => e.DisplayName, entity.DisplayName)
                .Set(e => e.Host, entity.Host)
                .Set(e => e.Port, entity.Port)
                .Set(e => e.UserName, entity.UserName)
                .Set(e => e.Password, entity.Password)
                .Set(e => e.EnableSsl, entity.EnableSsl)
                .Set(e => e.UseDefaultCredentials, entity.UseDefaultCredentials)
                .Set(e => e.IsDefault, entity.IsDefault)
                .CurrentDate(s => s.UpdatedOnUtc);

            return await _emailAccounts.UpdatePartiallyAsync(id, update);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _emailAccounts.DeleteAsync(id);
        }
    }
}
