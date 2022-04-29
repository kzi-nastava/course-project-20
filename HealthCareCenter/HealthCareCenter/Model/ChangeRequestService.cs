using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Model
{
    class ChangeRequestService
    {
        public static void DeleteAppointment(AppointmentChangeRequest changeRequest)
        {
            if (AppointmentRepository.AllAppointments == null)
            {
                return;
            }

            foreach (Appointment appointment in AppointmentRepository.AllAppointments)
            {
                if (appointment.ID == changeRequest.AppointmentID)
                {
                    AppointmentRepository.AllAppointments.Remove(appointment);
                    break;
                }
            }
        }

        public static void MakeChangesToAppointment(AppointmentChangeRequest changeRequest)
        {
            if (AppointmentRepository.AllAppointments == null)
            {
                return;
            }

            foreach (Appointment appointment in AppointmentRepository.AllAppointments)
            {
                if (appointment.ID == changeRequest.AppointmentID)
                {
                    appointment.AppointmentDate = changeRequest.NewDate;
                    appointment.Type = changeRequest.NewAppointmentType;
                    appointment.DoctorID = changeRequest.NewDoctorID;
                    break;
                }
            }
        }
    }
}
