using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Model
{
    class AppointmentChangeRequestService
    {
        public static void DeleteAppointment(AppointmentChangeRequest request)
        {
            if (AppointmentRepository.Appointments == null)
            {
                return;
            }

            foreach (Appointment appointment in AppointmentRepository.Appointments)
            {
                if (appointment.ID == request.AppointmentID)
                {
                    AppointmentRepository.Appointments.Remove(appointment);
                    break;
                }
            }
        }

        public static void MakeChangesToAppointment(AppointmentChangeRequest request)
        {
            if (AppointmentRepository.Appointments == null)
            {
                return;
            }

            foreach (Appointment appointment in AppointmentRepository.Appointments)
            {
                if (appointment.ID == request.AppointmentID)
                {
                    appointment.AppointmentDate = request.NewDate;
                    appointment.Type = request.NewAppointmentType;
                    appointment.DoctorID = request.NewDoctorID;
                    break;
                }
            }
        }
    }
}
