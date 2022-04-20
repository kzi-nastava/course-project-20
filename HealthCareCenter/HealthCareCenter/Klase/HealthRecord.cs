using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter
{
    class HealthRecord
    {
        public double height { get; set; }
        public double weight { get; set; }
        public List<string> previousDiseases { get; set; }
        public List<string> allergens { get; set; }
        public Patient patient { get; set; }
        public List<Appointment> allAppointments { get; set; }
    }
}
