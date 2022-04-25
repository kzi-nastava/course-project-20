using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using HealthCareCenter.Model;
using Newtonsoft.Json;


namespace HealthCareCenter.Model
{
    internal class HealthRecordsMenager
    {
        public static List<HealthRecord> HealthRecords{ get; set; }
        public static List<HealthRecord> loadHealthRecords()
        {
            var settings = new JsonSerializerSettings
            {
                DateFormatString = Constants.DateTimeFormat
            };

            String JSONTextHealthRecords = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\healthRecords.json");
            HealthRecords = (List<HealthRecord>)JsonConvert.DeserializeObject<IEnumerable<HealthRecord>>(JSONTextHealthRecords, settings);
            return HealthRecords;
        }
    }
}
