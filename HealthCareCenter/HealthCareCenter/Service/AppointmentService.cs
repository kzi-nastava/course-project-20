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
            if (AppointmentRepository.Appointments == null)
            {
                return null;
            }

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

        public static List<Appointment> GetPatientUnfinishedAppointments(int patientHealthRecordID)
        {
            if (AppointmentRepository.Appointments == null)
            {
                return null;
            }

            List<Appointment> unfinishedAppointments = new List<Appointment>();
            foreach (Appointment potentialAppointment in AppointmentRepository.Appointments)
            {
                if (potentialAppointment.HealthRecordID == patientHealthRecordID)
                {
                    if (potentialAppointment.ScheduledDate.CompareTo(DateTime.Now) > 0)
                    {
                        unfinishedAppointments.Add(potentialAppointment);
                    }
                }
            }

            return unfinishedAppointments;
        }

        public static List<Appointment> GetPatientFinishedAppointments(int patientHealthRecordID)
        {
            if (AppointmentRepository.Appointments == null)
            {
                return null;
            }

            List<Appointment> finishedAppointments = new List<Appointment>();
            foreach (Appointment potentialAppointment in AppointmentRepository.Appointments)
            {
                if (potentialAppointment.HealthRecordID == patientHealthRecordID)
                {
                    if (potentialAppointment.ScheduledDate.CompareTo(DateTime.Now) < 0)
                    {
                        finishedAppointments.Add(potentialAppointment);
                    }
                }
            }

            return finishedAppointments;
        }
    }
}
