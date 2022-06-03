using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Model
{
    public class MedicineCreationRequestRepository
    {
        public static List<MedicineCreationRequest> Requests = Load();
        public static int LargestID { get; set; }

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
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}