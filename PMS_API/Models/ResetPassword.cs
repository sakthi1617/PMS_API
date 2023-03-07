using System.ComponentModel.DataAnnotations;

namespace PMS_API.Models
{
    public class ResetPassword
    {
        public string? NewPassword { get; set; }
       
        public string? ConfirmPassword { get; set; }
    }
}
