using HealthCareCenter.Core.Appointments.Models;
using System.Collections.Generic;

namespace HealthCareCenter.Core.Appointments.Urgent.DTO
{
    public class OccupiedAppointmentsDTO
    {
        public List<Appointment> OccupiedAppointments { get; set; }
        public Dictionary<int, Appointment> NewAppointments { get; set; }

        public OccupiedAppointmentsDTO()
        {
            OccupiedAppointments = new List<Appointment>();
            NewAppointments = new Dictionary<int, Appointment>();
        }
    }
}
