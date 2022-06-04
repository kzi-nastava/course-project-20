using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Secretary
{
    /// <summary>
    /// Class used only for displaying referrals of a specific patient in PatientReferralsWindow
    /// </summary>
    public class PatientReferralForDisplay
    {
        public int ID { get; set; }
        public string DoctorUsername { get; set; }
        public string DoctorFirstName { get; set; }
        public string DoctorLastName { get; set; }

        public PatientReferralForDisplay() { }
        public PatientReferralForDisplay(int id)
        {
            ID = id;
        }
    }
}
