using HealthCareCenter.Enums;
using HealthCareCenter.Model;
using HealthCareCenter.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Secretary.Controllers
{
    public class ScheduleUrgentAppointmentController
    {
        private BaseUrgentAppointmentService _urgentAppointmentService;

        public ScheduleUrgentAppointmentController(BaseUrgentAppointmentService service)
        {
            AppointmentRepository.Load();
            _urgentAppointmentService = service;
        }

        public bool TryScheduling(AppointmentType type, string doctorType, Patient patient)
        {
            return _urgentAppointmentService.TryScheduling(type, doctorType, patient);
        }

        public List<string> GetTypesOfDoctors()
        {
            return DoctorService.GetTypesOfDoctors();
        }
    }
}
