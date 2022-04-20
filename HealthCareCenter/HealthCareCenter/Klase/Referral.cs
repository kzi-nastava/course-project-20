using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter
{
    public class Referral
    {
        public Patient _patient { get; set; }
        public List<Doctor> _doctors { get; set; }
    }
}