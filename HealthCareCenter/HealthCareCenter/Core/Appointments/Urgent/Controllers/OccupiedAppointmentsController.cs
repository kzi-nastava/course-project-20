using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Appointments.Services;
using HealthCareCenter.Core.Appointments.Urgent.Services;
using HealthCareCenter.Core.Patients;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Appointments.Urgent.Controllers
{
    public class OccupiedAppointmentsController
    {
        private BaseUrgentAppointmentService _service;

        public OccupiedAppointmentsController(BaseUrgentAppointmentService service)
        {
            _service = service;
        }

        public Appointment Postpone(ref string notification, Patient patient, AppointmentType type, OccupiedAppointment appointmentDisplay)
        {
            return _service.Postpone(ref notification, patient, type, appointmentDisplay);
        }

        public List<OccupiedAppointment> GetAppointmentsForDisplay()
        {
            return _service.GetAppointmentsForDisplay();
        }

        public void SortAppointments()
        {
            _service.SortPostponableAppointments();
        }
    }
}
