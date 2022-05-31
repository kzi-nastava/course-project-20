using HealthCareCenter.Model;
using HealthCareCenter.SecretaryGUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Service
{
    public class AppointmentService
    {
        public static Appointment Find(AppointmentDisplay appointmentDisplay)
        {
            foreach (Appointment appointment in AppointmentRepository.Appointments)
            {
                if (appointment.ID == appointmentDisplay.ID)
                {
                    return appointment;
                }
            }
            return null;
        }

        public static List<Appointment> GetAppointmentsInTheFollowingDays(DateTime date, int numberOfDays)
        {
            List<Appointment> appointments = new List<Appointment>();
            foreach (Appointment appointment in AppointmentRepository.Appointments)
            {
                TimeSpan timeSpan = appointment.ScheduledDate.Subtract(date);
                if (timeSpan.TotalDays <= 3 && timeSpan.TotalDays >= 0)
                {
                    appointments.Add(appointment);
                }
            }
            return appointments;
        } 
    }
}
