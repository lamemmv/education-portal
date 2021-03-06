﻿using EP.Data.DbContext;
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

        public async Task<IPagedList<QueuedEmail>> GetPagedListAsync(
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
                filter &= Builders<QueuedEmail>.Filter.Gte(e => e.CreatedOn, createdFromUtc);
            }

            if (createdToUtc.HasValue)
            {
                filter &= Builders<QueuedEmail>.Filter.Lte(e => e.CreatedOn, createdToUtc);
            }

            if (loadNotSentItemsOnly)
            {
                filter &= Builders<QueuedEmail>.Filter.Exists(e => e.SentOn, false);
            }

            if (loadOnlyItemsToBeSent)
            {
                var nowUtc = DateTime.UtcNow;
                filter &= Builders<QueuedEmail>.Filter.Exists(e => e.DontSendBeforeDate, false) |
                    Builders<QueuedEmail>.Filter.Lte(e => e.DontSendBeforeDate, nowUtc);
            }

            var sort = loadNewest ?
                Builders<QueuedEmail>.Sort.Descending(e => e.CreatedOn) :
                Builders<QueuedEmail>.Sort.Descending(e => e.Priority).Ascending(e => e.CreatedOn);

            return await _queuedEmails.GetPagedListAsync(filter, sort, skip: page, take: size);
        }

        public async Task<QueuedEmail> CreateAsync(QueuedEmail entity)
            => await _queuedEmails.CreateAsync(entity);

        public async Task<bool> UpdateAsync(string id, int sentTries, DateTime? sentOnUtc, string failedReason)
        {
            var update = Builders<QueuedEmail>.Update
                .Set(e => e.SentTries, sentTries)
                .Set(e => e.SentOn, sentOnUtc)
                .Set(e => e.FailedReason, failedReason)
                .Unset(e => e.EmailAccount);

            return await _queuedEmails.UpdatePartiallyAsync(id, update);
        }
    }
}
