using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.SecretaryGUI
{
    public class DeleteAppointmentChangeRequestDisplay
    {
        public int ID { get; set; }
        public DateTime TimeSent { get; set; }
        public string DoctorUsername { get; set; }
        public DateTime AppointmentTime { get; set; }
    }
}
