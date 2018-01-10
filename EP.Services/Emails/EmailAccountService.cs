using EP.Data.DbContext;
using EP.Data.Entities.Emails;
using EP.Data.Paginations;
using EP.Data.Repositories;
using EP.Services.Caching;
using EP.Services.Models;
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

        public async Task<ApiServerResult> CreateAsync(EmailAccount entity)
        {
            await _emailAccounts.CreateAsync(entity);

            if (entity.IsDefault)
            {
                _memoryCacheService.Remove(DefaultEmailAccount);
            }

            return ApiServerResult.Created(entity.Id);
        }

        public async Task<ApiServerResult> UpdateAsync(EmailAccount entity)
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
                .CurrentDate(e => e.UpdatedOn);

            var result = await _emailAccounts.UpdatePartiallyAsync(entity.Id, update);

            if (!result)
            {
                return ApiServerResult.NotFound();
            }

            _memoryCacheService.Remove(DefaultEmailAccount);

            return ApiServerResult.NoContent();
        }

        public async Task<ApiServerResult> DeleteAsync(string id)
        {
            var projection = Builders<EmailAccount>.Projection.Include(e => e.IsDefault);
            var entity = await _emailAccounts.DeleteAsync(id, projection);

            if (entity == null)
            {
                return ApiServerResult.NotFound();
            }

            if (entity.IsDefault)
            {
                _memoryCacheService.Remove(DefaultEmailAccount);
            }

            return ApiServerResult.NoContent();
        }
    }
}
