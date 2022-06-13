using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Core.HealthRecords
{
    public static class HealthRecordRepository
    {
        public static int maxID = -1;

        public static void CalculateMaxID()
        {
            maxID = -1;
            foreach (HealthRecord record in Records)
            {
                if (record.ID > maxID)
                {
                    maxID = record.ID;
                }
            }
        }

        private static List<HealthRecord> _records;
        public static List<HealthRecord> Records
        {
            get
            {
                if (_records == null)
                {
                    Load();
                }
                return _records;
            }
            set => _records = value;
        }

        public static void Load()
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
