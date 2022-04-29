using HealthCareCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Service
{
    class ChangeRequestService
    {
        public static void DeleteAppointment(AppointmentChangeRequest changeRequest)
        {
            if (AppointmentRepository.Appointments == null)
            {
                return;
            }

            foreach (Appointment appointment in AppointmentRepository.Appointments)
            {
                if (appointment.ID == changeRequest.AppointmentID)
                {
                    AppointmentRepository.Appointments.Remove(appointment);
                    break;
                }
            }
        }

        public static void MakeChangesToAppointment(AppointmentChangeRequest changeRequest)
        {
            if (AppointmentRepository.Appointments == null)
            {
                return;
            }

            foreach (Appointment appointment in AppointmentRepository.Appointments)
            {
                if (appointment.ID == changeRequest.AppointmentID)
                {
                    appointment.ScheduledDate = changeRequest.NewDate;
                    appointment.Type = changeRequest.NewAppointmentType;
                    appointment.DoctorID = changeRequest.NewDoctorID;
                    break;
                }
            }
        }
    }
}