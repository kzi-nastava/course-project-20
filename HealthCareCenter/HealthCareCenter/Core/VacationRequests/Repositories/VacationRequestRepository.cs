using HealthCareCenter.Core;
using HealthCareCenter.Core.VacationRequests.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Core.VacationRequests.Repositories
{
    public class VacationRequestRepository : BaseVacationRequestRepository
    {
        public VacationRequestRepository()
        {
            Load();
        }

        public override void Load()
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    DateFormatString = Constants.DateFormat
                };

                string JSONTextRequests = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\vacationRequests.json");
                Requests = (List<VacationRequest>)JsonConvert.DeserializeObject<IEnumerable<VacationRequest>>(JSONTextRequests, settings);
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
                    DateFormatString = Constants.DateFormat
                };

                using (StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\vacationRequests.json"))
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
