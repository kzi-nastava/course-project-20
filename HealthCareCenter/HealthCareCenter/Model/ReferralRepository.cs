﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Model
{
    public static class ReferralRepository
    {
        public static List<Referral> Referrals { get; set; }

        public static void Load()
        {
            try
            {
                string JSONTextReferrals = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\referrals.json");
                Referrals = (List<Referral>)JsonConvert.DeserializeObject<IEnumerable<Referral>>(JSONTextReferrals);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Save()
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