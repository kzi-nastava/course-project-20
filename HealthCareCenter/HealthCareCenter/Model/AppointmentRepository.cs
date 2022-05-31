using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HealthCareCenter.Model;
using Newtonsoft.Json;

namespace HealthCareCenter.Model
{
    class AppointmentRepository
    {
        private static List<Appointment> _appointments;
        public static List<Appointment> Appointments
        {
            get
            {
                if (_appointments == null)
                {
                    Load();
                }
                return _appointments;
            }
        }
        public static int LargestID { get; set; }

        public static List<Appointment> Load()
        {
            var settings = new JsonSerializerSettings
            {
                DateFormatString = Constants.DateTimeFormat
            };

            string JSONTextAppointments = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\appointments.json");
            _appointments = (List<Appointment>)JsonConvert.DeserializeObject<IEnumerable<Appointment>>(JSONTextAppointments, settings);
            LargestID = _appointments.Count == 0 ? 0 : _appointments[^1].ID;
            return _appointments;
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

        public static int GetLargestID()
        {
            int largestID = -1;
            foreach (Appointment appointment in Appointments)
            {
                if (appointment.ID > largestID)
                {
                    largestID = appointment.ID;
                }
            }

            return largestID;
        }
    }
}