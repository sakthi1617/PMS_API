using MimeKit;
using System.Net.Mail;

namespace PMS_API.SupportModel
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }

        public IFormFileCollection Attachments { get; set; }
     //   public List<IFormFile> attachment { get; set; }  

        public List<filedata> ContentBytes { get; set; }
        public Message(IEnumerable<string> to, string subject, string content, IFormFileCollection attachments,  List<filedata> contentBytes)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(X => new MailboxAddress("email", X)));
            Subject = subject;
            Content = content;
            Attachments = attachments;            
            ContentBytes = contentBytes;
        }
    }
    public class filedata
    {
        public string FileName { get; set; }
        public byte[] ContentBytes { get; set; }
    }

}