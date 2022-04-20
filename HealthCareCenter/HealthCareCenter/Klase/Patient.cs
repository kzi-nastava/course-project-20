using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Enums;

namespace HealthCareCenter
{
    public class Patient : User
    {
        public bool _isBlocked { get; set; }
        public Blocker _blocker { get; set; }
        public List<Referral> _referrals { get; set; }
        public List<Doctor> _doctors { get; set; }
        public HealthRecord _healthRecord { get; set; }
        public List<Prescription> _prescriptions { get; set; }
        public List<Survey> _surveys { get; set; }
    }
}