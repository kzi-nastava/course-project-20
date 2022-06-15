using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Core.HealthRecords
{
    public class HealthRecordRepository : BaseHealthRecordRepository
    {
        public override int CalculateMaxID()
        {
            LargestID = -1;
            foreach (HealthRecord record in Records)
            {
                if (record.ID > LargestID)
                {
                    LargestID = record.ID;
                }
            }
            return LargestID;
        }

        public override void Load()
        {
            try
            {
                string JSONTextHealthRecords = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\healthRecords.json");
                _records = (List<HealthRecord>)JsonConvert.DeserializeObject<IEnumerable<HealthRecord>>(JSONTextHealthRecords);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void Save()
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
