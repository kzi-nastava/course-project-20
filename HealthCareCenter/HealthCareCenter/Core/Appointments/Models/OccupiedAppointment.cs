using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Appointments.Models
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

        public AppointmentDisplay() { }

        public AppointmentDisplay(int id, AppointmentType type, DateTime scheduledDate, bool emergency, DateTime postponedTime)
        {
            ID = id;
            Type = type;
            ScheduledDate = scheduledDate;
            Emergency = emergency;
            PostponedTime = postponedTime;
        }
    }
}
