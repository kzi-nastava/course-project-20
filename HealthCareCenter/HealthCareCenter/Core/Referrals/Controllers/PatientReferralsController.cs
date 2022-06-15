using HealthCareCenter.Core.Patients;
using HealthCareCenter.Core.Referrals.Models;
using HealthCareCenter.Core.Referrals.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Referrals.Controllers
{
    public class PatientReferralsController
    {
        private readonly IReferralService _referralsService;

        public PatientReferralsController(IReferralService service)
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
