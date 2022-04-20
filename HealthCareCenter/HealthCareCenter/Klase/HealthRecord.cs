using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter
{
    public class HealthRecord
    {
        public double _height { get; set; }
        public double _weight { get; set; }
        public List<string> _previousDiseases { get; set; }
        public List<string> _allergens { get; set; }
        public Patient _patient { get; set; }
        public List<Appointment> _appointments { get; set; }
    }
}