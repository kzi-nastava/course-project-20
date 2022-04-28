using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Model
{
    class AppointmentRepository
    {
        public static List<Appointment> AllAppointments { get; set; }
        public static int LargestID { get; set; }

        public static List<Appointment> Load()
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    DateFormatString = Constants.DateTimeFormat
                };

                String JSONTextAppointments = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\appointments.json");
                AllAppointments = (List<Appointment>)JsonConvert.DeserializeObject<IEnumerable<Appointment>>(JSONTextAppointments, settings);
                if (AllAppointments.Count == 0)
                {
                    LargestID = 0;
                }
                else
                {
                    LargestID = AllAppointments[^1].ID;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return AllAppointments;
        }

        public static List<Appointment> GetPatientUnfinishedAppointments(int patientHealthRecordID)
        {
            List<Appointment> unfinishedAppointments = new List<Appointment>();
            foreach (Appointment potentialAppointment in AllAppointments)
            {
                if (potentialAppointment.HealthRecordID == patientHealthRecordID)
                {
                    if (potentialAppointment.AppointmentDate.CompareTo(DateTime.Now) > 0)
                    {
                        unfinishedAppointments.Add(potentialAppointment);
                    }
                }
            }

            return unfinishedAppointments;
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
                using (StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\appointments.json"))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, AllAppointments);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
