using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Appointments.Urgent
{
    /// <summary>
    /// Class used only for displaying occupied appointments in OccupiedAppointmentsWindow
    /// </summary>
    public class OccupiedAppointment
    {
        public int ID { get; set; }
        public AppointmentType Type { get; set; }
        public DateTime ScheduledDate { get; set; }
        public bool Emergency { get; set; }
        public string DoctorName { get; set; }
        public string PatientName { get; set; }
        public DateTime PostponedTime { get; set; }

        public OccupiedAppointment() { }

        public OccupiedAppointment(int id, AppointmentType type, DateTime scheduledDate, bool emergency, DateTime postponedTime)
        {
            ID = id;
            Type = type;
            ScheduledDate = scheduledDate;
            Emergency = emergency;
            PostponedTime = postponedTime;
        }
    }
}
