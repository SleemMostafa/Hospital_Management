using System;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Management.DTO
{
    public class AppointmentModel
    {
        [Required]
        public DateTime Time { get; set; }
        [Required]
        public int PatientID { get; set; }
      
    }
}
