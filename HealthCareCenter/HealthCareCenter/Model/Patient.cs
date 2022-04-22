using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Enums;

namespace HealthCareCenter
{
    public class Patient : User
    {
        public bool IsBlocked { get; set; }
        public Blocker BlockedBy { get; set; }
        public List<int> ReferralIDs { get; set; }
        public int HealthRecordIDs { get; set; }
        public List<int> PrescriptionIDs { get; set; }
        //public List<Survey> Surveys { get; set; }
    }
}