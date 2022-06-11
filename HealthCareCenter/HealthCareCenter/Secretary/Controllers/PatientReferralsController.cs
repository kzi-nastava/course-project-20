using HealthCareCenter.Model;
using HealthCareCenter.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Secretary.Controllers
{
    public class PatientReferralsController
    {
        private IReferralsService _referralsService;

        public PatientReferralsController(IReferralsService service)
        {
            _referralsService = service;
        }

        public List<PatientReferralForDisplay> Get(Patient patient)
        {
            return _referralsService.Get(patient);
        }

        public Referral Get(int referralID)
        {
            return _referralsService.Get(referralID);
        }
    }
}
