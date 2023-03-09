using System.ComponentModel.DataAnnotations;

namespace PMS_API.SupportModel
{
    public class ResetPassword
    {
        public string? NewPassword { get; set; }

        public string? ConfirmPassword { get; set; }
    }
}
