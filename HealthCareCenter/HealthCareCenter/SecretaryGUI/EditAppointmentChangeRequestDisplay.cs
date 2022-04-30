using HealthCareCenter.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.SecretaryGUI
{
    public class EditAppointmentChangeRequestDisplay
    {
        public int ID { get; set; }
        public DateTime TimeSent { get; set; }
        public string OriginalDoctorUsername { get; set; }
        public string NewDoctorUsername { get; set; }
        public DateTime OriginalAppointmentTime { get; set; }
        public DateTime NewAppointmentTime { get; set; }
        public AppointmentType OriginalAppointmentType { get; set; }
        public AppointmentType NewAppointmentType { get; set; }
    }
}
