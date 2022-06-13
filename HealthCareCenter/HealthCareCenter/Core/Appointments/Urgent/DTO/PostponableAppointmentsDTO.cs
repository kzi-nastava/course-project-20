using HealthCareCenter.Core.Appointments.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Appointments.Urgent.DTO
{
    public class PostponableAppointmentsDTO
    {
        public List<Appointment> OccupiedAppointments { get; set; }
        public Dictionary<int, Appointment> NewAppointments { get; set; }
        public Dictionary<int, DateTime> AppointmentPostponableTo { get; set; }

        public PostponableAppointmentsDTO(OccupiedAppointmentsDTO appointments)
        {
            OccupiedAppointments = appointments.OccupiedAppointments;
            NewAppointments = appointments.NewAppointments;
            AppointmentPostponableTo = new Dictionary<int, DateTime>();
        }
    }
}
