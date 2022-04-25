using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HealthCareCenter.Model;
using Newtonsoft.Json;

namespace HealthCareCenter.Model
{
    internal class AppointmentsMenager
    {
        public static List<Appointment> Appointments { get; set; }
        public static int HighestIndex { get; set; }
        public static List<Appointment> loadAppointments()
        {
            var settings = new JsonSerializerSettings
            {
                DateFormatString = Constants.DateFormat
            };

            String JSONTextAppointments = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\appointments.json");
            Appointments = (List<Appointment>)JsonConvert.DeserializeObject<IEnumerable<Appointment>>(JSONTextAppointments, settings);
            HighestIndex = Appointments[Appointments.Count-1].ID;
            return Appointments;
        }
    }
}
