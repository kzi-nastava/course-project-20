using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Model
{
    class ChangeRequestRepository
    {
        public static List<AppointmentChangeRequest> AllChangeRequests{ get; set; }
        public static int LargestID { get; set; }
        
        public static List<AppointmentChangeRequest> Load()
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    DateFormatString = Constants.DateTimeFormat
                };

                String JSONTextAppointments = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\changerequests.json");
                AllChangeRequests = (List<AppointmentChangeRequest>)JsonConvert.DeserializeObject<IEnumerable<AppointmentChangeRequest>>(JSONTextAppointments, settings);
                if (AllChangeRequests.Count == 0)
                {
                    LargestID = 0;
                }
                else
                {
                    LargestID = AllChangeRequests[^1].ID;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return AllChangeRequests;
        }

        public static void Write()
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer
                {
                    Formatting = Formatting.Indented,
                    DateFormatString = Constants.DateTimeFormat
                };
                using (StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\changerequests.json"))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, AllChangeRequests);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
