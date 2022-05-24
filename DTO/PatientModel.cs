using Hospital_Management.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Management.DTO
{
    public enum Status
    {
        cancel = 0,
        confirm =1
    }
    public class PatientModel
    {
        [Required]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Error name must letter only and more than 3 letter")]
        [MinLength(3)]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public int Age { get; set; }
    }
}
