using System.Threading.Tasks;

namespace EP.Services.Emails
{
    public interface IEmailSender
    {
        Task SendEmailAsync(
            EmailSetting emailSetting,
            string from,
            string toCsv,
            string subject,
            string body,
            bool htmlBody = true,
            string fromAlias = null,
            string toAlias = null,
            string replyTo = null,
            string replyToAlias = null,
            string ccCsv = null,
            string bccCsv = null);
    }
}
