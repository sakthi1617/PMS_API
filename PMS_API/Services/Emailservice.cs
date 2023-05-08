using MailKit.Net.Smtp;
using MimeKit;
using PMS_API.Repository;
using PMS_API.SupportModel;

namespace PMS_API.Services
{
    public class Emailservice : IEmailService
    {
        private readonly EmailConfig _emailConfig;
        public Emailservice(EmailConfig emailConfig)
        {
            _emailConfig = emailConfig;
        }
        public string SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            var a = Send(emailMessage);
            if(a == "MailSend")
            {
                return "ok";
            }

            return "Error";
        }
        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            //emailMessage.Body = new TextPart("html")
            //{
            //    Text = message.Content
            //};
            // var bodyBuilder = new BodyBuilder { HtmlBody = string.Format("<h2 style='color:red;'>{0}</h2>", message.Content) };
            //Text = $"To set your password, please click the following link: https://localhost:7099/api/OrganizationAuth/GeneratetPassword?Email=" + message.To[0].Address

            var bodyBuilder = new BodyBuilder { HtmlBody = string.Format("<h2 style='color:green;'>{0}</h3>", message.Content) };
            if (message.Attachments != null && message.Attachments.Any())
            {
                byte[] fileBytes;
                foreach (var attachment in message.Attachments)
                {
                    using (var ms = new MemoryStream())
                    {
                        attachment.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }
                    bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
                }
            }
            emailMessage.Body = bodyBuilder.ToMessageBody();

            return emailMessage;
        }
        private string Send(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfig.userName, _emailConfig.Password);
                var res = client.Send(mailMessage);                   
         
                return "MailSend";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }
    }
}

