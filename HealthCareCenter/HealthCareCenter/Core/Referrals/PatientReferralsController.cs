using HealthCareCenter.Core.Patients.Models;
using HealthCareCenter.Core.Referrals.Models;
using HealthCareCenter.Core.Referrals.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Referrals
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
