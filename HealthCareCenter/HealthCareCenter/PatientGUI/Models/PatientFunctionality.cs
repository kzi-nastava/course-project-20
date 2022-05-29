using HealthCareCenter.Model;
using HealthCareCenter.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.PatientGUI.Models
{
    class PatientFunctionality
    {
        private static PatientFunctionality instance;

        private PatientFunctionality() { }

        public static PatientFunctionality GetInstance()
        {
            return instance is null ? new PatientFunctionality() : instance;
        }

        public List<AppointmentTerm> GetAllPossibleTermsForCreateAppointment(int chosenDoctorID, DateTime chosenScheduleDate)
        {
            List<AppointmentTerm> allPossibleTerms = AppointmentTermService.GetDailyTermsFromRange(Constants.StartWorkTime, 0, Constants.EndWorkTime, 0);
            foreach (Appointment appointment in AppointmentRepository.Appointments)
            {
                if (appointment.DoctorID == chosenDoctorID &&
                    appointment.ScheduledDate.Date.CompareTo(chosenScheduleDate.Date) == 0)
                {
                    AppointmentTerm unavailableSchedule = new AppointmentTerm(appointment.ScheduledDate.Hour, appointment.ScheduledDate.Minute);
                    allPossibleTerms.Remove(unavailableSchedule);
                }
            }

            return allPossibleTerms;
        }

        public void ScheduleAppointment(DateTime scheduleDate, int doctorID, int healthRecordID, int hospitalRoomID)
        {
            if (CheckCreationTroll())
            {
                return;
            }

            Appointment newAppointment = new Appointment
            {
                ID = ++AppointmentRepository.LargestID,
                Type = Enums.AppointmentType.Checkup,
                CreatedDate = DateTime.Now,
                ScheduledDate = scheduleDate,
                Emergency = false,
                DoctorID = doctorID,
                HealthRecordID = healthRecordID,
                HospitalRoomID = hospitalRoomID,
                PatientAnamnesis = null
            };
            HospitalRoomService.AddAppointmentToRoom(hospitalRoomID, newAppointment.ID);
            AppointmentRepository.Appointments.Add(newAppointment);

            AppointmentRepository.Save();
        }

        public bool CheckCreationTroll()
        {
            return false;
        }

    }
}
