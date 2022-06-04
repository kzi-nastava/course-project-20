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
            UrgentAppointmentService.UrgentInfo = info;
        }

        public bool TryScheduling(AppointmentType type, string doctorType, Patient patient)
        {
            return UrgentAppointmentService.TryScheduling(type, doctorType, patient);
        }

        public List<string> GetTypesOfDoctors()
        {
            return DoctorService.GetTypesOfDoctors();
        }
    }
}
