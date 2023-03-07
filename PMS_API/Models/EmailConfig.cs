namespace PMS_API.Models
{
    public class EmailConfig
    {
       
            public string From { get; set; } = null!;
            public string SmtpServer { get; set; } = null!;
            public int Port { get; set; }
            public string userName { get; set; } = null!;
            public string Password { get; set; } = null!;
       
    }
}
