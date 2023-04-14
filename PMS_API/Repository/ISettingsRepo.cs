using PMS_API.ViewModels;

namespace PMS_API.Repository
{
    public interface ISettingsRepo
    {
        public string TimeSetting(TimeSettingVM model);
    }
}
