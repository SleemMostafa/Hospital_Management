using System.ComponentModel.DataAnnotations;

namespace Hospital_Management.DTO
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "NewPassword is required")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "ComfirmNewPassword is required")]
        public string ConfirmNewPassword { get; set; }
        public string Token { get; set; }
    }
}
