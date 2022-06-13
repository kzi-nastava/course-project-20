using HealthCareCenter.Core.Referrals.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Referrals.Repositories
{
    public abstract class BaseReferralRepository
    {
        public List<Referral> Referrals { get; set; }
        public int LargestID { get; set; }
        public abstract List<Referral> Load();
        public abstract void Save();
    }
}