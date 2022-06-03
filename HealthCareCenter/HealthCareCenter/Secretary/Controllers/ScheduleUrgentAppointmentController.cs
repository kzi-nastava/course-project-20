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

        public bool TryScheduling(AppointmentType type, string doctorType, Patient patient, ref List<Appointment> occupiedAppointments, ref Dictionary<int, Appointment> newAppointmentsInfo)
        {
            return AppointmentService.TryScheduling(type, doctorType, patient, ref occupiedAppointments, ref newAppointmentsInfo);
        }

        public List<string> GetTypesOfDoctors()
        {
            return UserService.GetTypesOfDoctors();
        }
    }
}
