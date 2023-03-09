using MimeKit;

namespace PMS_API.SupportModel
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }

        public Message(IEnumerable<string> to, string subject, string content)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(X => new MailboxAddress("email", X)));
            Subject = subject;
            Content = content;
        }
    }
}
