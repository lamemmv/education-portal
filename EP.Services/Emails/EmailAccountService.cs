using EP.Data.DbContext;
using EP.Data.Entities.Emails;
using EP.Data.Paginations;
using EP.Data.Repositories;
using EP.Services.Caching;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace EP.Services.Emails
{
    public sealed class EmailAccountService : IEmailAccountService
    {
        private const string DefaultEmailAccountKey = "Cache.DefaultEmailAccount";

        private readonly IRepository<EmailAccount> _emailAccounts;
        private readonly IMemoryCacheService _memoryCacheService;

        public EmailAccountService(
            MongoDbContext dbContext,
            IMemoryCacheService memoryCacheService)
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

        public async Task<EmailAccount> FindDefaultAsync()
        {
            return await _memoryCacheService.GetSlidingExpiration(
                DefaultEmailAccountKey,
                () =>
                {
                    var filter = Builders<EmailAccount>.Filter.Eq(e => e.IsDefault, true);
                    var project = Builders<EmailAccount>.Projection.Exclude(e => e.IsDefault);

                    return _emailAccounts.FindAsync(filter, project);
                });
        }

        public async Task<EmailAccount> CreateAsync(EmailAccount entity)
        {
            return await _emailAccounts.CreateAsync(entity);
        }

        public async Task<bool> UpdateAsync(EmailAccount entity)
        {
            entity = await _emailAccounts.UpdateAsync(entity);

            return entity != null;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _emailAccounts.DeleteAsync(id);
        }
    }
}
