using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Model
{
    public class MedicineChangeRequestRepository
    {
        private const string _fileName = "medicineChangeRequests.json";
        public static List<MedicineChangeRequest> Requests = Load();

        public static List<MedicineChangeRequest> Load()
        {
            try
            {
                List<MedicineChangeRequest> requests = new List<MedicineChangeRequest>();
                var settings = new JsonSerializerSettings
                {
                    DateFormatString = Constants.DateFormat
                };

                string JSONTextChangeMedicineRequests = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\" + _fileName);
                requests = (List<MedicineChangeRequest>)JsonConvert.DeserializeObject<IEnumerable<MedicineChangeRequest>>(JSONTextChangeMedicineRequests, settings);
                return requests;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Save()
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer
                {
                    Formatting = Formatting.Indented
                };

                using (StreamWriter sw = new StreamWriter(@"..\..\..\data\" + _fileName))
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