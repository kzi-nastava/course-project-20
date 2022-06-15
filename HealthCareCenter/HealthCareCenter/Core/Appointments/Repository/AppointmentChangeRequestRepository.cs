using HealthCareCenter.Core.Appointments.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace HealthCareCenter.Core.Appointments.Repository
{
    class AppointmentChangeRequestRepository : BaseAppointmentChangeRequestRepository
    {
        public override List<AppointmentChangeRequest> Load()
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    DateFormatString = Constants.DateTimeFormat
                };

                string JSONTextRequests = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\appointmentChangeRequests.json");
                _requests = (List<AppointmentChangeRequest>)JsonConvert.DeserializeObject<IEnumerable<AppointmentChangeRequest>>(JSONTextRequests, settings);
                LargestID = _requests.Count == 0 ? 0 : _requests[^1].ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _requests;
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
                using (StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\appointmentChangeRequests.json"))
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

        public override int GetLargestID()
        {
            int largestID = -1;
            foreach (AppointmentChangeRequest request in Requests)
            {
                if (request.ID > largestID)
                {
                    largestID = request.ID;
                }
            }

            return largestID;
        }
    }
}