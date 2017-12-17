using EP.Data.DbContext;
using EP.Data.Entities.Emails;
using EP.Data.Paginations;
using EP.Data.Repositories;
using EP.Services.Extensions;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace EP.Services.Emails
{
    public sealed class QueuedEmailService : IQueuedEmailService
    {
        private readonly IRepository<QueuedEmail> _queuedEmails;

        public QueuedEmailService(MongoDbContext dbContext)
        {
            _queuedEmails = dbContext.QueuedEmails;
        }

        public async Task<IPagedList<QueuedEmail>> FindAsync(
            DateTime? createdFromUtc,
            DateTime? createdToUtc,
            bool loadNotSentItemsOnly,
            bool loadOnlyItemsToBeSent,
            int maxSendTries,
            bool loadNewest,
            int page,
            int size)
        {
            var filter = Builders<QueuedEmail>.Filter.Lt(e => e.SentTries, maxSendTries);

            if (createdFromUtc.HasValue)
            {
                var startDate = createdFromUtc.Value.StartOfDay();
                filter &= Builders<QueuedEmail>.Filter.Gte(e => e.CreatedOnUtc, startDate);
            }

            if (createdToUtc.HasValue)
            {
                var endDate = createdToUtc.Value.EndOfDay();
                filter &= Builders<QueuedEmail>.Filter.Lte(e => e.CreatedOnUtc, endDate);
            }

            if (loadNotSentItemsOnly)
            {
                filter &= Builders<QueuedEmail>.Filter.Eq("sentonutc", BsonNull.Value);
            }

            if (loadOnlyItemsToBeSent)
            {
                DateTime nowUtc = DateTime.UtcNow;
                filter &= Builders<QueuedEmail>.Filter.Eq("dontsendbeforedateutc", BsonNull.Value) |
                    Builders<QueuedEmail>.Filter.Lte(e => e.DontSendBeforeDateUtc, nowUtc);
            }

            var sort = loadNewest ?
                Builders<QueuedEmail>.Sort.Descending(e => e.CreatedOnUtc) :
                Builders<QueuedEmail>.Sort.Descending(e => e.Priority).Ascending(e => e.CreatedOnUtc);

            return await _queuedEmails.FindAsync(filter, sort, skip: page, take: size);
        }

        public async Task<QueuedEmail> CreateAsync(QueuedEmail entity)
        {
            return await _queuedEmails.CreateAsync(entity);
        }

        public async Task<bool> UpdateAsync(string id, int sentTries, DateTime? sentOnUtc, string failedReason)
        {
            var update = Builders<QueuedEmail>.Update
                .Set(e => e.SentTries, sentTries)
                .Set(e => e.SentOnUtc, sentOnUtc)
                .Set(e => e.FailedReason, failedReason)
                .CurrentDate(s => s.UpdatedOnUtc);

            return await _queuedEmails.UpdatePartiallyAsync(id, update);
        }
    }
}
