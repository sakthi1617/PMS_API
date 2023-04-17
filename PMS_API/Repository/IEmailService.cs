using PMS_API.SupportModel;
using PMS_API.ViewModels;

namespace PMS_API.Repository
{
    public interface IEmailService
    {
        string SendEmail(Message message);
    }
}
