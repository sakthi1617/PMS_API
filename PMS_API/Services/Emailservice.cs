using MailKit.Net.Smtp;
using MimeKit;
using PMS_API.LogHandling;
using PMS_API.Reponse;
using PMS_API.Repository;
using PMS_API.SupportModel;
using System.IO;
using System.Net.Mail;

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

            emailMessage.Body = new TextPart("html")
            {
                Text = message.Content
            };
            var bodyBuilder = new BodyBuilder { HtmlBody = string.Format("<p style='color:red;'>{0}</p>", message.Content) };
            // Text = $"To set your password, please click the following link: https://localhost:7099/api/OrganizationAuth/GeneratetPassword?Email=" + message.To[0].Address

            //var bodyBuilder = new BodyBuilder { HtmlBody = string.Format("<h2 style='color:green;'>{0}</h3>", message.Content) };
            //if (message.Attachments != null && message.Attachments.Any())
            //{
            //    byte[] fileBytes;
            //    foreach (var attachment in message.Attachments)
            //    {
            //        using (var ms = new MemoryStream())
            //        {
            //            attachment.CopyTo(ms);
            //            fileBytes = ms.ToArray();
            //            ms.Dispose();
            //        }
            
            //    }
            //}

            if (message.ContentBytes != null && message.ContentBytes.Any())
            {
                byte[] fileBytess;
                List<MimePart> AttachmentsList = new List<MimePart>();
                foreach (var item in message.ContentBytes)
                {
                    try
                    {
                        //var attachment = new MimePart("text", "txt")
                        //{
                        //    Content = new MimeContent(File.OpenRead(item.FileName)),
                        //    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                        //    ContentTransferEncoding = ContentEncoding.Base64,
                        //    FileName = Path.GetFileName(item.FileName)
                        //};
                        //AttachmentsList.Add(attachment);
                        bodyBuilder.Attachments.Add(item.FileName, item.ContentBytes);
                        //bodyBuilder.Attachments.Add(item.FileName, item.ContentBytes, ContentType.Parse(attachment.ContentType));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }


                // now set the multipart/mixed as the message body
                emailMessage.Body = bodyBuilder.ToMessageBody();
            }            
            return emailMessage;
        }
        private string Send(MimeMessage mailMessage)
        {
            using var client = new MailKit.Net.Smtp.SmtpClient();
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

