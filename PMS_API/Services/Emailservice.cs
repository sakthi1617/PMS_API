﻿using MailKit.Net.Smtp;
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

        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
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


            //Text = $"To set your password, please click the following link: https://localhost:7099/api/OrganizationAuth/GeneratetPassword?Email=" + message.To[0].Address





            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfig.userName, _emailConfig.Password);

                client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }


}

