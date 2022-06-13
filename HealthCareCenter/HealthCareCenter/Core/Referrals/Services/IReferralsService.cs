using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Patients.Models;
using HealthCareCenter.Core.Referrals.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Referrals.Services
{
    public interface IReferralsService
    {
        List<PatientReferralForDisplay> Get(Patient patient);
        Referral Get(int referralID);
        void Schedule(Referral referral, Appointment appointment);
        void Fill(int doctorID, int patientID, Referral referral);
    }
}
