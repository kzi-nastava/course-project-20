using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.SecretaryGUI
{
    /// <summary>
    /// Class used only for displaying referrals of a specific patient in PatientReferralsWindow
    /// </summary>
    class PatientReferral
    {
        public int ID { get; set; }
        public string DoctorUsername { get; set; }
        public string DoctorFirstName { get; set; }
        public string DoctorLastName { get; set; }
    }
}
