using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Model
{
    class AppointmentChangeRequestRepository
    {
        public static List<AppointmentChangeRequest> Requests { get; set; }
        public static int LargestID { get; set; }

        public static List<AppointmentChangeRequest> Load()
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    DateFormatString = Constants.DateTimeFormat
                };

                string JSONTextAppointments = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\appointmentChangeRequests.json");
                Requests = (List<AppointmentChangeRequest>)JsonConvert.DeserializeObject<IEnumerable<AppointmentChangeRequest>>(JSONTextAppointments, settings);
                LargestID = Requests.Count == 0 ? 0 : Requests[^1].ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
    }
}