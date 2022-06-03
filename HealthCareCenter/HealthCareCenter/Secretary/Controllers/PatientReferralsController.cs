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

        public List<PatientReferral> Get(Patient patient)
        {
            return ReferralsService.Get(patient);
        }

        public Referral Find(int referralID)
        {
            return ReferralsService.Find(referralID);
        }
    }
}
