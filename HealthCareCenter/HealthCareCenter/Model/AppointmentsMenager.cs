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
        public List<Appointment> Appointments { get; set; }
        public List<Appointment> loadAppointments()
        {
            var settings = new JsonSerializerSettings
            {
                DateFormatString = Constants.DateFormat
            };

            String JSONTextAppointments = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\appointments.json");
            Appointments = (List<Appointment>)JsonConvert.DeserializeObject<IEnumerable<Appointment>>(JSONTextAppointments, settings);
            return Appointments;
        }
    }
}
