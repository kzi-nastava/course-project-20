using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Model;

namespace HealthCareCenter.Service
{
    public static class ReferralService
    {
        public static void FillReferral(int doctorID,int patientID, Referral referral)
        {
            referral.ID = ReferralRepository.LargestID;
            referral.DoctorID = doctorID;
            referral.PatientID = patientID;
        }
    }
}
