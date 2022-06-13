using HealthCareCenter.Core.Referrals.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace HealthCareCenter.Core.Referrals.Repositories
{
    public class ReferralRepository : BaseReferralRepository
    {
        public ReferralRepository()
        {
            Load();
        }

        public override List<Referral> Load()
        {
            string JSONTextReferrals = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\referrals.json");
            Referrals = (List<Referral>)JsonConvert.DeserializeObject<IEnumerable<Referral>>(JSONTextReferrals);
            LargestID = Referrals.Count == 0 ? 0 : Referrals[^1].ID;
            return Referrals;
        }

        public override void Save()
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer
                {
                    Formatting = Formatting.Indented
                };
                using (StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\referrals.json"))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, Referrals);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
