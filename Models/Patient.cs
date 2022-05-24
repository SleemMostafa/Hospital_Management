using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hospital_Management.Models
{
    public class Patient
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Age { get; set; }
        [JsonIgnore]
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
