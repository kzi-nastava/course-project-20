using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Model
{
    public static class ReferralRepository
    {
        public static List<Referral> Referrals { get; set; }
        public static int LargestID { get; set; }
        public static List<Referral> Load()
        {
            var settings = new JsonSerializerSettings
            {
                DateFormatString = Constants.TimeFormat
            };

            string JSONTextReferrals = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\referrals.json");
            Referrals = (List<Referral>)JsonConvert.DeserializeObject<IEnumerable<Referral>>(JSONTextReferrals, settings);
            LargestID = Referrals.Count == 0 ? 0 : Referrals[^1].ID;
            return Referrals;
        }
        public static void Save()
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer
                {
                    Formatting = Formatting.Indented,
                    DateFormatString = Constants.TimeFormat
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
