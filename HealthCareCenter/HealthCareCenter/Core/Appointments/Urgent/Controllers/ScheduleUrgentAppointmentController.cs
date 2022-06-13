using HealthCareCenter.Core.Appointments.Repository;
using HealthCareCenter.Core.Appointments.Services;
using HealthCareCenter.Core.Appointments.Urgent.Services;
using HealthCareCenter.Core.Patients;
using HealthCareCenter.Core.Users.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Appointments.Urgent.Controllers
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
