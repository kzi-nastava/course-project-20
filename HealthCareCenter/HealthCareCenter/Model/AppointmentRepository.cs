using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HealthCareCenter.Model;
using Newtonsoft.Json;

namespace HealthCareCenter.Model
{
    internal class AppointmentRepository
    {
        public static List<Appointment> Appointments { get; set; }
        public static int LargestID { get; set; }

        public static List<Appointment> Load()
        {
            var settings = new JsonSerializerSettings
            {
                DateFormatString = Constants.DateTimeFormat
            };

            String JSONTextAppointments = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\appointments.json");
            Appointments = (List<Appointment>)JsonConvert.DeserializeObject<IEnumerable<Appointment>>(JSONTextAppointments, settings);
            LargestID = Appointments.Count == 0 ? 0 : Appointments[^1].ID;
            return Appointments;
        }
      
        public static List<Appointment> GetPatientUnfinishedAppointments(int patientHealthRecordID)
        {
            List<Appointment> unfinishedAppointments = new List<Appointment>();
            foreach (Appointment potentialAppointment in Appointments)
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

        public static void Save()
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
                    serializer.Serialize(writer, Appointments);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
