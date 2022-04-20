using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Patient
{
    public class Patient : User
    {
        public boolean isBlocked { get; set; }
        public Blocker blocker { get; set; }
        public List<Referral> referrals { get; set; }
        public List<Doctor> doctors { get; set; }
        public HealthRecord healthRecord { get; set; }
        public List<Prescription> allPrescriptions { get; set; }
        public List<Survey> surveys { get; set; }
    }
}
