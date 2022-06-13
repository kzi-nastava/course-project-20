using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Appointments.Services;
using HealthCareCenter.Core.Patients.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Appointments.Controllers
{
    public class OccupiedAppointmentsController
    {
        public BaseUrgentAppointmentService _service;

        public OccupiedAppointmentsController(BaseUrgentAppointmentService service)
        {
            _service = service;
        }

        public Appointment Postpone(ref string notification, Patient patient, AppointmentType type, AppointmentDisplay appointmentDisplay)
        {
            return _service.Postpone(ref notification, patient, type, appointmentDisplay);
        }

        public List<AppointmentDisplay> GetAppointmentsForDisplay()
        {
            return _service.GetAppointmentsForDisplay();
        }

        public void SortAppointments()
        {
            _service.SortPostponableAppointments();
        }
    }
}
