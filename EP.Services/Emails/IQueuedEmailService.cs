using EP.Data.Entities.Emails;
using EP.Data.Paginations;
using System;
using System.Threading.Tasks;

namespace EP.Services.Emails
{
    public interface IQueuedEmailService
    {
        Task<IPagedList<QueuedEmail>> FindAsync(
           DateTime? createdFromUtc,
            DateTime? createdToUtc,
            bool loadNotSentItemsOnly,
            bool loadOnlyItemsToBeSent,
            int maxSendTries,
            bool loadNewest,
            int page,
            int size);

        Task<QueuedEmail> CreateAsync(QueuedEmail entity);

        Task<bool> UpdateAsync(string id, int sentTries, DateTime? sentOnUtc, string failedReason);
    }
}
