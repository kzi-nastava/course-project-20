using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Appointments.Models
{
    public class UrgentAppointmentInfo
    {
        public List<Appointment> OccupiedAppointments { get; set; }
        public Dictionary<int, Appointment> NewAppointmentsInfo { get; set; }

        public UrgentAppointmentInfo()
        {
            OccupiedAppointments = new List<Appointment>();
            NewAppointmentsInfo = new Dictionary<int, Appointment>();
        }

        public UrgentAppointmentInfo(List<Appointment> occupiedAppointments, Dictionary<int, Appointment> newAppointmentsInfo)
        {
            OccupiedAppointments = occupiedAppointments;
            NewAppointmentsInfo = newAppointmentsInfo;
        }
    }
}
