using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EP.Services.Emails
{
    public sealed class NetEmailSender : IEmailSender
    {
        private static readonly char[] Seperators = new char[] { ',', ';' };

        public async Task SendEmailAsync(
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
            string bccCsv = null)
        {
            ValidateEmail(emailSetting, from, toCsv, subject);

            var emailMessage = BuildMimeMessage(from, fromAlias, toCsv, toAlias, subject, body, htmlBody, replyTo, replyToAlias, ccCsv, bccCsv);

            await SendEmailAsync(emailMessage, emailSetting);
        }

        private void ValidateEmail(EmailSetting emailSetting, string from, string to, string subject)
        {
            if (emailSetting == null)
            {
                throw new ArgumentException($"{nameof(NetEmailSender)}: No Email Setting provided.");
            }

            if (string.IsNullOrEmpty(from))
            {
                throw new ArgumentException($"{nameof(NetEmailSender)}: No From address provided.");
            }

            if (string.IsNullOrEmpty(to))
            {
                throw new ArgumentException($"{nameof(NetEmailSender)} No To address provided.");
            }

            if (string.IsNullOrEmpty(subject))
            {
                throw new ArgumentException($"{nameof(NetEmailSender)} No Subject provided.");
            }
        }

        private MailMessage BuildMimeMessage(
            string from,
            string fromAlias,
            string toCsv,
            string toAlias,
            string subject,
            string body,
            bool htmlBody,
            string replyTo,
            string replyToAlias,
            string ccCsv,
            string bccCsv)
        {
            var emailMessage = new MailMessage();

            AddFrom(emailMessage, from, fromAlias);

            var toAddresses = toCsv.Split(Seperators, StringSplitOptions.RemoveEmptyEntries);

            if (toAddresses.Length == 1)
            {
                AddTo(emailMessage, toCsv, toAlias);
            }
            else
            {
                AddTo(emailMessage, toAddresses);
            }

            if (!string.IsNullOrEmpty(replyTo))
            {
                AddReplyTo(emailMessage, replyTo, replyToAlias);
            }

            if (!string.IsNullOrEmpty(ccCsv))
            {
                AddCc(emailMessage, ccCsv.Split(Seperators, StringSplitOptions.RemoveEmptyEntries));
            }

            if (!string.IsNullOrEmpty(bccCsv))
            {
                AddBcc(emailMessage, bccCsv.Split(Seperators, StringSplitOptions.RemoveEmptyEntries));
            }

            emailMessage.Subject = subject;

            AddBody(emailMessage, body, htmlBody);

            return emailMessage;
        }

        private MailMessage AddFrom(MailMessage emailMessage, string from, string fromAlias = null)
        {
            emailMessage.From = new MailAddress(from, fromAlias ?? string.Empty);

            return emailMessage;
        }

        private MailMessage AddTo(MailMessage emailMessage, string to, string toAlias = null)
        {
            emailMessage.To.Add(new MailAddress(to, toAlias ?? string.Empty));

            return emailMessage;
        }

        private MailMessage AddTo(MailMessage emailMessage, string[] to)
        {
            emailMessage.To.Concat(to.Select(addr => new MailAddress(addr)));

            return emailMessage;
        }

        private MailMessage AddReplyTo(MailMessage emailMessage, string replyTo, string replyToAlias = null)
        {
            emailMessage.ReplyToList.Add(new MailAddress(replyTo, replyToAlias ?? string.Empty));

            return emailMessage;
        }

        private MailMessage AddCc(MailMessage emailMessage, params string[] cc)
        {
            emailMessage.CC.Concat(cc.Select(addr => new MailAddress(addr)));

            return emailMessage;
        }
        private MailMessage AddBcc(MailMessage emailMessage, params string[] bcc)
        {
            emailMessage.Bcc.Concat(bcc.Select(addr => new MailAddress(addr)));

            return emailMessage;
        }

        private MailMessage AddBody(MailMessage emailMessage, string body, bool htmlBody)
        {
            emailMessage.Body = body;
            emailMessage.IsBodyHtml = htmlBody;

            return emailMessage;
        }

        private async Task SendEmailAsync(MailMessage emailMessage, EmailSetting emailSetting)
        {
            using (var client = new SmtpClient())
            {
                if (emailSetting.UseDefaultCredentials)
                {
                    client.Credentials = new NetworkCredential(emailSetting.UserName, emailSetting.Password);
                }

                client.EnableSsl = emailSetting.EnableSsl;
                await client.SendMailAsync(emailMessage);
            }
        }
    }
}
