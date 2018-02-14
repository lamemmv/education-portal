using EP.Data.DbContext;
using EP.Data.Entities.Emails;
using EP.Data.Entities;
using EP.Data.Paginations;
using EP.Data.Repositories;
using EP.Services.Caching;
using EP.Services.Constants;
using EP.Services.Logs;
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
        private readonly IActivityLogService _activityLogService;

        public EmailAccountService(
            MongoDbContext dbContext,
            IMemoryCacheService memoryCacheService,
            IActivityLogService activityLogService)
        {
            _emailAccounts = dbContext.EmailAccounts;
            _memoryCacheService = memoryCacheService;
            _activityLogService = activityLogService;
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

        public async Task<EmailAccount> CreateAsync(EmailAccount entity, EmbeddedUser embeddedUser, string ip)
        {
            await _emailAccounts.CreateAsync(entity);

            if (entity.IsDefault)
            {
                _memoryCacheService.Remove(DefaultEmailAccount);
            }

            // Activity Log.
            await _activityLogService.CreateAsync(SystemKeyword.CreateEmailAccount, entity, embeddedUser, ip);

            return entity;
        }

        public async Task<bool> UpdateAsync(EmailAccount entity, EmbeddedUser embeddedUser, string ip)
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

            if (result)
            {
                _memoryCacheService.Remove(DefaultEmailAccount);

                // Activity Log.
                await _activityLogService.CreateAsync(SystemKeyword.UpdateEmailAccount, entity, embeddedUser, ip);
            }

            return result;
        }

        public async Task<bool> DeleteAsync(string id, EmbeddedUser embeddedUser, string ip)
        {
            var oldEntity = await _emailAccounts.DeleteAsync(id, null);

            if (oldEntity != null)
            {
                if (oldEntity.IsDefault)
                {
                    _memoryCacheService.Remove(DefaultEmailAccount);
                }

                // Activity Log.
                await _activityLogService.CreateAsync(SystemKeyword.DeleteEmailAccount, oldEntity, embeddedUser, ip);

                return true;
            }

            return false;
        }
    }
}
