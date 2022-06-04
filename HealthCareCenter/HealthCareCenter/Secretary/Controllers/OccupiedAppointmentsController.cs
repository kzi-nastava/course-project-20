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
        public OccupiedAppointmentsController(OccupiedAppointmentInfo info)
        {
            UrgentAppointmentService.OccupiedInfo = info;
        }

        public Appointment Postpone(ref string notification, Patient patient, AppointmentType type, AppointmentDisplay appointmentDisplay)
        {
            return UrgentAppointmentService.Postpone(ref notification, patient, type, appointmentDisplay);
        }

        public List<AppointmentDisplay> GetAppointmentsForDisplay()
        {
            return UrgentAppointmentService.GetAppointmentsForDisplay();
        }

        public void SortAppointments()
        {
            UrgentAppointmentService.SortPostponableAppointments();
        }
    }
}
