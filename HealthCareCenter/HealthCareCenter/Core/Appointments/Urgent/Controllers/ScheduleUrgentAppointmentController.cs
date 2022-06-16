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
        private readonly BaseUrgentAppointmentService _urgentAppointmentService;
        private readonly IDoctorService _doctorService;
        
        public ScheduleUrgentAppointmentController(
            BaseUrgentAppointmentService urgentAppointmentService,
            IDoctorService doctorService)
        {
            _urgentAppointmentService = urgentAppointmentService;
            _doctorService = doctorService;
        }

        public bool TryScheduling(AppointmentType type, string doctorType, Patient patient)
        {
            return _urgentAppointmentService.TryScheduling(type, doctorType, patient);
        }

        public List<string> GetTypesOfDoctors()
        {
            return _doctorService.GetTypesOfDoctors();
        }
    }
}
