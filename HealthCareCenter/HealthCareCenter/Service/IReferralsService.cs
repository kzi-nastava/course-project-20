using HealthCareCenter.Model;
using HealthCareCenter.Secretary;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Service
{
    public interface IReferralsService
    {
        List<PatientReferralForDisplay> Get(Patient patient);
        Referral Get(int referralID);
        void Schedule(Referral referral, Appointment appointment);
        void Fill(int doctorID, int patientID, Referral referral);
    }
}
