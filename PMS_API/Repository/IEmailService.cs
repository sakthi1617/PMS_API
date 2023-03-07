using PMS_API.Models;

namespace PMS_API.Repository
{
    public interface IEmailService
    {
        void SendEmail(Message message);

    }
}
