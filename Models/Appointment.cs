using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Hospital_Management.Models
{
    public class Appointment
    {
        public int ID { get; set; }
        public DateTime Time { get; set; }
        [ForeignKey("Product")]
        public int PatientID { get; set; }
        [JsonIgnore]
        public virtual Patient Patient { get; set; }
    }
}
