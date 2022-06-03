using HealthCareCenter.Enums;
using HealthCareCenter.Model;
using HealthCareCenter.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Secretary.Controllers
{
    public class OccupiedAppointmentsController
    {
        public Appointment Postpone(ref string notification, Patient patient, Dictionary<int, Appointment> newAppointmentsInfo, AppointmentType type, AppointmentDisplay appointmentDisplay)
        {
            return AppointmentService.Postpone(ref notification, patient, newAppointmentsInfo, type, appointmentDisplay);
        }

        public List<AppointmentDisplay> GetAppointmentsForDisplay(List<Appointment> occupiedAppointments, Dictionary<int, DateTime> newDateOf)
        {
            return AppointmentService.GetAppointmentsForDisplay(occupiedAppointments, newDateOf);
        }

        public void SortAppointments(ref List<Appointment> occupiedAppointments, ref Dictionary<int, DateTime> dateOf)
        {
            AppointmentService.SortAppointments(ref occupiedAppointments, ref dateOf);
        }
    }
}
