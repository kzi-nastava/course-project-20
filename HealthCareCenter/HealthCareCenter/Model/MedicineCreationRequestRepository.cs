using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Model
{
    {
        public static List<MedicineCreationRequest> Requests{ get; set; }
        public static int LargestID { get; set; }
        private const string _fileName = "medicineCreationRequests.json";
        public static List<Medicine> Medicines = Load();

        public static List<MedicineCreationRequest> Load()
        {
                var settings = new JsonSerializerSettings
                {
                DateFormatString = Constants.DateTimeFormat
                };

            string JSONTextAppointments = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\medicineCreationRequests.json");
            Requests = (List<MedicineCreationRequest>)JsonConvert.DeserializeObject<IEnumerable<MedicineCreationRequest>>(JSONTextAppointments, settings);
            LargestID = Requests.Count == 0 ? 0 : Requests[^1].ID;
            return Requests;
        }

        public static void Save()
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer
                {
                    Formatting = Formatting.Indented,
                    DateFormatString = Constants.DateTimeFormat
                };
                using (StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\medicineCreationRequests.json"))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, Requests);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}