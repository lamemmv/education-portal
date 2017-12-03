//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using Apex.Core.Models;
//using MimeKit;
//using MailKit;

//namespace EP.Services.Emails
//{
//    public class MailKitEmailSender : IEmailSender
//    {
//        private static readonly char[] Seperators = new char[] { ',', ';' };

//        public async Task SendEmailAsync(
//            EmailSetting emailSetting,
//            string from,
//            string toCvs,
//            string subject,
//            string body,
//            bool htmlBody = true,
//            string fromAlias = null,
//            string toAlias = null,
//            string replyTo = null,
//            string replyToAlias = null,
//            string ccCsv = null,
//            string bccCsv = null)
//        {
//            ValidateEmail(emailSetting, from, toCvs, subject);

//            var emailMessage = BuildMimeMessage(from, fromAlias, toCvs, toAlias, subject, body, htmlBody, replyTo, replyToAlias, ccCsv, bccCsv);

//            await SendEmailAsync(emailMessage, emailSetting);
//        }

//        //public void SendEmailAsyncViaSMTPPickupFolder(string email, string subject, string message)
//        //{
//        //	var emailMessage = new MimeMessage();

//        //	emailMessage.From.Add(new MailboxAddress("Joe Bloggs", "jbloggs@example.com"));
//        //	emailMessage.To.Add(new MailboxAddress("", email));
//        //	emailMessage.Subject = subject;
//        //	emailMessage.Body = new TextPart("plain") { Text = message };

//        //	using (StreamWriter data = System.IO.File.CreateText("c:\\smtppickup\\email.txt"))
//        //	{
//        //		emailMessage.WriteTo(data.BaseStream);
//        //	}
//        //}

//        private void ValidateEmail(EmailSetting emailSetting, string from, string to, string subject)
//        {
//            if (emailSetting == null)
//            {
//                throw new ArgumentException($"{nameof(EmailSender)}: No Email Setting provided.");
//            }

//            if (string.IsNullOrEmpty(from))
//            {
//                throw new ArgumentException($"{nameof(MailKitEmailSender)}: No From address provided.");
//            }

//            if (string.IsNullOrEmpty(to))
//            {
//                throw new ArgumentException($"{nameof(MailKitEmailSender)} No To address provided.");
//            }

//            if (string.IsNullOrEmpty(subject))
//            {
//                throw new ArgumentException($"{nameof(MailKitEmailSender)} No Subject provided.");
//            }
//        }

//        private MimeMessage BuildMimeMessage(
//            string from,
//            string fromAlias,
//            string toCsv,
//            string toAlias,
//            string subject,
//            string body,
//            bool htmlBody,
//            string replyTo,
//            string replyToAlias,
//            string ccCsv,
//            string bccCsv)
//        {
//            var emailMessage = new MimeMessage();

//            AddFrom(emailMessage, from, fromAlias);

//            var toAddresses = toCsv.Split(Seperators, StringSplitOptions.RemoveEmptyEntries);

//            if (toAddresses.Length == 1)
//            {
//                AddTo(emailMessage, toCsv, toAlias);
//            }
//            else
//            {
//                AddTo(emailMessage, toAddresses);

//                emailMessage.Importance = MessageImportance.High;
//            }

//            if (!string.IsNullOrEmpty(replyTo))
//            {
//                AddReplyTo(emailMessage, replyTo, replyToAlias);
//            }

//            if (!string.IsNullOrEmpty(ccCsv))
//            {
//                AddCc(emailMessage, ccCsv.Split(Seperators, StringSplitOptions.RemoveEmptyEntries));
//            }

//            if (!string.IsNullOrEmpty(bccCsv))
//            {
//                AddBcc(emailMessage, bccCsv.Split(Seperators, StringSplitOptions.RemoveEmptyEntries));
//            }

//            emailMessage.Subject = subject;

//            AddBody(emailMessage, body, htmlBody);

//            return emailMessage;
//        }

//        private MimeMessage AddFrom(MimeMessage emailMessage, string from, string fromAlias = null)
//        {
//            emailMessage.From.Add(new MailboxAddress(fromAlias ?? string.Empty, from));

//            return emailMessage;
//        }

//        private MimeMessage AddTo(MimeMessage emailMessage, string to, string toAlias = null)
//        {
//            emailMessage.To.Add(new MailboxAddress(toAlias ?? string.Empty, to));

//            return emailMessage;
//        }

//        private MimeMessage AddTo(MimeMessage emailMessage, string[] to)
//        {
//            emailMessage.To.AddRange(to.Select(addr => new MailboxAddress(addr)));

//            return emailMessage;
//        }

//        private MimeMessage AddReplyTo(MimeMessage emailMessage, string replyTo, string replyToAlias = null)
//        {
//            emailMessage.ReplyTo.Add(new MailboxAddress(replyToAlias ?? string.Empty, replyTo));

//            return emailMessage;
//        }

//        private MimeMessage AddCc(MimeMessage emailMessage, params string[] cc)
//        {
//            emailMessage.Cc.AddRange(cc.Select(addr => new MailboxAddress(addr)));

//            return emailMessage;
//        }

//        private MimeMessage AddBcc(MimeMessage emailMessage, params string[] bcc)
//        {
//            emailMessage.Bcc.AddRange(bcc.Select(addr => new MailboxAddress(addr)));

//            return emailMessage;
//        }

//        private MimeMessage AddBody(MimeMessage emailMessage, string body, bool htmlBody)
//        {
//            BodyBuilder bodyBuilder = new BodyBuilder();

//            if (htmlBody)
//            {
//                bodyBuilder.HtmlBody = body;
//            }
//            else
//            {
//                bodyBuilder.TextBody = body;
//            }

//            emailMessage.Body = bodyBuilder.ToMessageBody();

//            return emailMessage;
//        }

//        private async Task SendEmailAsync(MimeMessage emailMessage, EmailSetting emailSetting)
//        {
//            using (var client = new SmtpClient())
//            {
//                await client.ConnectAsync(emailSetting.Host, emailSetting.Port, emailSetting.EnableSsl);

//                // Note: since we don't have an OAuth2 token, disable the XOAUTH2 authentication mechanism.
//                client.AuthenticationMechanisms.Remove("XOAUTH2");

//                // Note: only needed if the SMTP server requires authentication.
//                if (emailSetting.UseDefaultCredentials)
//                {
//                    await client.AuthenticateAsync(emailSetting.UserName, emailSetting.Password);
//                }

//                await client.SendAsync(emailMessage);
//                await client.DisconnectAsync(true);
//            }
//        }
//    }
//}
