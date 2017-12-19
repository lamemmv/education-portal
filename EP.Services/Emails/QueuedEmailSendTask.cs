using EP.Data.Entities.Emails;
using EP.Services.SystemTasks;
using Serilog;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace EP.Services.Emails
{
    public sealed class QueuedEmailSendTask : BackgroundHostedService
    {
        private readonly ILogger _logger;
        private readonly IQueuedEmailService _queuedEmailService;
        private readonly IEmailSender _emailSender;

        public QueuedEmailSendTask(
            ILogger logger,
            IQueuedEmailService queuedEmailService,
            IEmailSender emailSender)
        {
            _logger = logger;
            _queuedEmailService = queuedEmailService;
            _emailSender = emailSender;
        }

        protected override float LoopInSeconds => 1;

        protected async override Task ExecuteOnceAsync(CancellationToken cancellationToken)
        {
            var pagedQueuedEmail = await _queuedEmailService.FindAsync(
                createdFromUtc: null,
                createdToUtc: null,
                loadNotSentItemsOnly: true,
                loadOnlyItemsToBeSent: true,
                maxSendTries: 3,
                loadNewest: false,
                page: 1,
                size: 512);

            foreach (var queuedEmail in pagedQueuedEmail.Items)
            {
                await SendEmailAsync(queuedEmail);
                await UpdateSentTriesAsync(queuedEmail);
            }
        }

        private async Task SendEmailAsync(QueuedEmail queuedEmail)
        {
            try
            {
                await _emailSender.SendEmailAsync(
                    queuedEmail.EmailAccount,
                    queuedEmail.From,
                    queuedEmail.To,
                    queuedEmail.Subject,
                    queuedEmail.Body,
                    true,
                    queuedEmail.FromName,
                    queuedEmail.ToName,
                    queuedEmail.ReplyTo,
                    queuedEmail.CC,
                    queuedEmail.BCC);

                queuedEmail.SentOn = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                queuedEmail.FailedReason = ex.ToString();

                _logger.Error(
                   ex,
                   "{Class} » {Method}: {Message}", nameof(QueuedEmailSendTask), nameof(SendEmailAsync), ex.Message);
            }
        }

        private async Task UpdateSentTriesAsync(QueuedEmail queuedEmail)
        {
            try
            {
                await _queuedEmailService.UpdateAsync(
                    queuedEmail.Id,
                    ++queuedEmail.SentTries,
                    queuedEmail.SentOn,
                    queuedEmail.FailedReason);
            }
            catch (Exception ex)
            {
                _logger.Error(
                   ex,
                   "{Class} » {Method}: {Message}", nameof(UpdateSentTriesAsync), nameof(UpdateSentTriesAsync), ex.Message);
            }
        }
    }
}
