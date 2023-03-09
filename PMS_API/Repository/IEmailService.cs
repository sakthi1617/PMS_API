using PMS_API.SupportModel;

namespace PMS_API.Repository
{
    public interface IEmailService
    {
        void SendEmail(Message message);

    }
}
