using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Model
{
    public static class HealthRecordRepository
    {
        public static List<HealthRecord> Records { get; set; }

        public static void Load()
        {
            try
            {
                String JSONTextHealthRecords = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\healthRecords.json");
                Records = (List<HealthRecord>)JsonConvert.DeserializeObject<IEnumerable<HealthRecord>>(JSONTextHealthRecords);
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

                using (StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\healthRecords.json"))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, Records);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
