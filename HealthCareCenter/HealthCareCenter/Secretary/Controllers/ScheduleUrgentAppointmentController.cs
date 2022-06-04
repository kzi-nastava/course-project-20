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
        public ScheduleUrgentAppointmentController()
        {
            AppointmentRepository.Load();
        }

        public ScheduleUrgentAppointmentController(UrgentAppointmentInfo info)
        {
            AppointmentRepository.Load();
            AppointmentService.UrgentInfo = info;
        }

        public bool TryScheduling(AppointmentType type, string doctorType, Patient patient)
        {
            return AppointmentService.TryScheduling(type, doctorType, patient);
        }

        public List<string> GetTypesOfDoctors()
        {
            return UserService.GetTypesOfDoctors();
        }
    }
}
