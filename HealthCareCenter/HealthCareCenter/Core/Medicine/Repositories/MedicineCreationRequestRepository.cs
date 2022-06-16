using HealthCareCenter.Core.Medicine.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Core.Medicine.Repositories
{
    public class MedicineCreationRequestRepository : AMedicineCreationRequestRepository
    {
        public int LargestID { get; set; }

        public MedicineCreationRequestRepository()
        {
            Requests = Load();
        }

        public override int GetLargestId()
        {
            try
            {
                List<MedicineCreationRequest> requests = Requests;
                requests.Sort((x, y) => x.ID.CompareTo(y.ID));
                if (requests.Count == 0)
                {
                    return -1;
                }

                return requests[requests.Count - 1].ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override List<MedicineCreationRequest> Load()
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

        public override void Save()
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