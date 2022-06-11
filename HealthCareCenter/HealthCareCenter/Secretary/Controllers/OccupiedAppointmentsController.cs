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
