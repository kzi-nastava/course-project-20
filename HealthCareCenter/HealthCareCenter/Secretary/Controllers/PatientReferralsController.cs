using HealthCareCenter.Model;
using HealthCareCenter.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Secretary.Controllers
{
    public class PatientReferralsController
    {
        public PatientReferralsController()
        {
            ReferralRepository.Load();
        }

        public List<PatientReferralForDisplay> Get(Patient patient)
        {
            return ReferralsService.Get(patient);
        }

        public Referral Get(int referralID)
        {
            return ReferralsService.Get(referralID);
        }
    }
}
