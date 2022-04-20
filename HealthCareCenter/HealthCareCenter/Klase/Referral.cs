using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter
{
    public class Referral
    {
        public Patient patient { get; set; }
        public List<Doctor> doctors { get; set; }
    }
}
