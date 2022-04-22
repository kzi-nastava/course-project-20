using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Model
{
    public class HealthRecord
    {
        public int ID { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public List<string> PreviousDiseases { get; set; }
        public List<string> Allergens { get; set; }
        public int PatientID { get; set; }
        public List<int> AppointmentIDs { get; set; }
    }
}