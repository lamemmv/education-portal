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

        public async Task<IPagedList<EmailAccount>> GetPagedListAsync(int? page, int? size)
        {
            var projection = Builders<EmailAccount>.Projection.Exclude(e => e.Password);

            return await _emailAccounts.GetPagedListAsync(projection: projection, skip: page, take: size);
        }

        public async Task<EmailAccount> GetByIdAsync(string id)
        {
            var projection = Builders<EmailAccount>.Projection.Exclude(e => e.Password);

            return await _emailAccounts.GetByIdAsync(id, projection);
        }

        public async Task<EmailAccount> FindDefaultAsync()
        {
            return await _memoryCacheService.GetOrAddSlidingExpiration(
                DefaultEmailAccount,
                () =>
                {
                    var filter = Builders<EmailAccount>.Filter.Eq(e => e.IsDefault, true);
                    var projection = Builders<EmailAccount>.Projection.Exclude(e => e.IsDefault);

                    return _emailAccounts.GetSingleAsync(filter, projection);
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
            var projection = Builders<EmailAccount>.Projection.Exclude(e => e.Password);
            var oldEntity = await _emailAccounts.UpdateAsync(entity, projection);

            if (oldEntity != null && entity.IsDefault)
            {
                _memoryCacheService.Remove(DefaultEmailAccount);
            }

            return oldEntity;
        }

        public async Task<EmailAccount> DeleteAsync(string id)
        {
            var projection = Builders<EmailAccount>.Projection.Exclude(e => e.Password);
            var entity = await _emailAccounts.DeleteAsync(id, projection);

            if (entity != null && entity.IsDefault)
            {
                _memoryCacheService.Remove(DefaultEmailAccount);
            }

            return entity;
        }
    }
}
