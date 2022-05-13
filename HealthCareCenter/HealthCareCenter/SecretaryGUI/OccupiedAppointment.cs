using HealthCareCenter.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.SecretaryGUI
{
    /// <summary>
    /// Class used only for displaying occupied appointments in OccupiedAppointmentsWindow
    /// </summary>
    public class AppointmentDisplay
    {
        public int ID { get; set; }
        public AppointmentType Type { get; set; }
        public DateTime ScheduledDate { get; set; }
        public bool Emergency { get; set; }
        public string DoctorName { get; set; }
        public string PatientName { get; set; }
        public DateTime PostponedTime { get; set; }
    }
}
