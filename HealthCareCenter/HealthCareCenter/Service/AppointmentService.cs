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
                AppointmentRepository.Load();
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

        public static Appointment Find(int appointmentID)
        {
            if (AppointmentRepository.Appointments == null)
            {
                AppointmentRepository.Load();
            }

            foreach (Appointment appointment in AppointmentRepository.Appointments)
            {
                if (appointment.ID == appointmentID)
                {
                    return appointment;
                }
            }
            return null;
        }

        public static List<Appointment> GetPatientUnfinishedAppointments(int patientHealthRecordID)
        {
            if (AppointmentRepository.Appointments == null)
            {
                AppointmentRepository.Load();
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
                AppointmentRepository.Load();
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
