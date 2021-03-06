﻿using EP.Data.Entities.Blobs;
using EP.Data.Entities.Emails;
using EP.Data.Entities.Logs;
using EP.Data.Entities.News;
using EP.Data.Repositories;
using MongoDB.Driver;

namespace EP.Data.DbContext
{
    public abstract class BaseDbContext
    {
        public IMongoDatabase MongoDatabase { get; set; }

        //public abstract void SetupCollections();

        public abstract IRepository<EmailAccount> EmailAccounts { get; }

        public abstract IRepository<QueuedEmail> QueuedEmails { get; }

        public abstract IRepository<ActivityLogType> ActivityLogTypes { get; }

        public abstract IRepository<ActivityLog> ActivityLogs { get; }

        public abstract IRepository<Log> Logs { get; }

        public abstract IRepository<Blob> Blobs { get; }

        public abstract IRepository<NewsItem> News { get; }
    }
}
