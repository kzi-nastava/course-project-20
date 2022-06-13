using HealthCareCenter.Core.Appointments.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace HealthCareCenter.Core.Appointments.Repository
{
    class AppointmentChangeRequestRepository
    {
        private static List<AppointmentChangeRequest> _requests;
        public static List<AppointmentChangeRequest> Requests
        {
            get
            {
                if (_requests == null)
                {
                    Load();
                }
                return _requests;
            }
        }
        public static int LargestID { get; set; }

        public static List<AppointmentChangeRequest> Load()
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    DateFormatString = Constants.DateTimeFormat
                };

                string JSONTextRequests = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\Data\appointmentChangeRequests.json");
                _requests = (List<AppointmentChangeRequest>)JsonConvert.DeserializeObject<IEnumerable<AppointmentChangeRequest>>(JSONTextRequests, settings);
                LargestID = _requests.Count == 0 ? 0 : _requests[^1].ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _requests;
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
                using (StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\Data\appointmentChangeRequests.json"))
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

        public static int GetLargestID()
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