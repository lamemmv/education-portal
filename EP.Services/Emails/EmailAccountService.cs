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
        private const string DefaultEmailAccount = "Cache." + nameof(DefaultEmailAccount);

        private readonly IRepository<EmailAccount> _emailAccounts;
        private readonly IMemoryCacheService _memoryCacheService;

        public EmailAccountService(
            MongoDbContext dbContext,
            IMemoryCacheService memoryCacheService)
        {
            _emailAccounts = dbContext.EmailAccounts;
            _memoryCacheService = memoryCacheService;
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
                DefaultEmailAccount,
                () =>
                {
                    var filter = Builders<EmailAccount>.Filter.Eq(e => e.IsDefault, true);
                    var options = new FindOptions<EmailAccount, EmailAccount>
                    {
                        Projection = Builders<EmailAccount>.Projection.Exclude(e => e.IsDefault)
                    };

                    return _emailAccounts.FindAsync(filter, options);
                });
        }

        public async Task<EmailAccount> CreateAsync(EmailAccount entity)
        {
            entity = await _emailAccounts.CreateAsync(entity);

            if (entity.IsDefault)
            {
                _memoryCacheService.Remove(DefaultEmailAccount);
            }

            return entity;
        }

        public async Task<EmailAccount> UpdateAsync(EmailAccount entity)
        {
            var options = new FindOneAndReplaceOptions<EmailAccount, EmailAccount>
            {
                ReturnDocument = ReturnDocument.Before
            };

            var oldEntity = await _emailAccounts.UpdateAsync(entity, options);

            if (oldEntity != null && entity.IsDefault)
            {
                _memoryCacheService.Remove(DefaultEmailAccount);
            }

            return oldEntity;
        }

        public async Task<EmailAccount> DeleteAsync(string id)
        {
            var entity = await _emailAccounts.DeleteAsync(id, options: null);

            if (entity != null && entity.IsDefault)
            {
                _memoryCacheService.Remove(DefaultEmailAccount);
            }

            return entity;
        }
    }
}
