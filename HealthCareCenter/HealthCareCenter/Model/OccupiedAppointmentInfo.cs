using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Model
{
    public class OccupiedAppointmentInfo
    {
        public List<Appointment> OccupiedAppointments { get; set; }
        public Dictionary<int, Appointment> NewAppointmentsInfo { get; set; }
        public Dictionary<int, DateTime> NewDateOf { get; set; }

        public OccupiedAppointmentInfo()
        {
            OccupiedAppointments = new List<Appointment>();
            NewAppointmentsInfo = new Dictionary<int, Appointment>();
            NewDateOf = new Dictionary<int, DateTime>();
        }

        public OccupiedAppointmentInfo(List<Appointment> occupiedAppointments, Dictionary<int, Appointment> newAppointmentsInfo, Dictionary<int, DateTime> newDateOf)
        {
            OccupiedAppointments = occupiedAppointments;
            NewAppointmentsInfo = newAppointmentsInfo;
            NewDateOf = newDateOf;
        }

        public OccupiedAppointmentInfo(UrgentAppointmentInfo info)
        {
            OccupiedAppointments = info.OccupiedAppointments;
            NewAppointmentsInfo = info.NewAppointmentsInfo;
            NewDateOf = new Dictionary<int, DateTime>();
        }
    }
}
