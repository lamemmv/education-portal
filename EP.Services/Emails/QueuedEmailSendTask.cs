using EP.Data.Entities.Emails;
using EP.Services.SystemTasks;
using ExpressMapper.Extensions;
using Serilog;
using System;
using System.Threading.Tasks;

namespace EP.Services.Emails
{
    public class QueuedEmailSendTask : BaseBackgroundTask, IBackgroundTask
    {
        private readonly ILogger _logger;
        private readonly IQueuedEmailService _queuedEmailService;
        private readonly IEmailSender _emailSender;

        public QueuedEmailSendTask(int taskId, int loopInSeconds)
            : base(taskId, loopInSeconds)
        {
        }

        public QueuedEmailSendTask(
            int taskId,
            int loopInSeconds,
            ILogger logger,
            IQueuedEmailService queuedEmailService,
            IEmailSender emailSender)
            : base(taskId, loopInSeconds)
        {
            _logger = logger;
            _queuedEmailService = queuedEmailService;
            _emailSender = emailSender;
        }

        public override async Task Execute()
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
                var emailSetting = queuedEmail.EmailAccount.Map<EmailAccount, EmailSetting>();

                await _emailSender.SendEmailAsync(
                    emailSetting,
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
                queuedEmail.SentTries = queuedEmail.SentTries + 1;

                await _queuedEmailService.UpdateAsync(
                    queuedEmail.Id,
                    queuedEmail.SentTries,
                    queuedEmail.SentOn,
                    queuedEmail.FailedReason);
            }
            catch (Exception ex)
            {
                _logger.Error(
                   ex,
                   "{Class} » {Method}: {Message}", nameof(UpdateSentTriesAsync), nameof(SendEmailAsync), ex.Message);
            }
        }
    }
}
