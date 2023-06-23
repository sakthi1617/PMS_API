using System.ComponentModel.DataAnnotations;

namespace PMS_API.SupportModel
{
    public class ResetPassword
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [StringLength(16,MinimumLength =8,ErrorMessage ="Pasword must contain minimun 8 Characters and maximum 16 Characters only")]
        public string? NewPassword { get; set; }
        [Required]
        public string? ConfirmPassword { get; set; }
    }
}
