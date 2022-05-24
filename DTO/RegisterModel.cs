using System.ComponentModel.DataAnnotations;

namespace Hospital_Management.DTO
{
    public class RegisterModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmePassword { set; get; }
    }
}
