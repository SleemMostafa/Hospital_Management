using System.ComponentModel.DataAnnotations;

namespace Hospital_Management.DTO
{
    public class TokenRequestModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}