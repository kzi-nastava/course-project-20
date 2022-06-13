using HealthCareCenter.Core;
using HealthCareCenter.Core.Equipment.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Core.Equipment.Repositories
{
    public class DynamicEquipmentRequestRepository : BaseDynamicEquipmentRequestRepository
    {
        public DynamicEquipmentRequestRepository()
        {
            Load();
            CalculateMaxID();
        }

        public override void CalculateMaxID()
        {
            maxID = -1;
            foreach (DynamicEquipmentRequest request in Requests)
            {
                if (request.ID > maxID)
                {
                    maxID = request.ID;
                }
            }
        }

        public override void Load()
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    DateFormatString = Constants.DateTimeFormat
                };

                string JSONTextRequests = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\Data\dynamicEquipmentRequests.json");
                Requests = (List<DynamicEquipmentRequest>)JsonConvert.DeserializeObject<IEnumerable<DynamicEquipmentRequest>>(JSONTextRequests, settings);
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
                    Formatting = Formatting.Indented,
                    DateFormatString = Constants.DateTimeFormat
                };

                using (StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\Data\dynamicEquipmentRequests.json"))
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
