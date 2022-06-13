using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Appointments.Repository;
using HealthCareCenter.Core.Rooms.Repositories;
using HealthCareCenter.Core.Rooms.Services;
using HealthCareCenter.Core.Users.Services;
using System;
using System.Collections.Generic;

namespace HealthCareCenter.Core.Appointments.Services
{
    public static class AppointmentService
    {

        public static Appointment Get(AppointmentDisplay appointmentDisplay)
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

        public static bool Schedule(Appointment appointment, bool checkTroll)
        {
            if (checkTroll && PatientService.CheckCreationTroll(PatientService.GetPatientByHealthRecordID(appointment.HealthRecordID)))
            {
                return false;
            }

            AppointmentRepository.Appointments.Add(appointment);
            AppointmentRepository.Save();

            HospitalRoomService.Update(appointment.HospitalRoomID, appointment);
            HospitalRoomRepository.Save();

            return true;
        }

        public static bool Schedule(DateTime scheduleDate, int doctorID, int healthRecordID, int hospitalRoomID)
        {
            if (PatientService.CheckCreationTroll(PatientService.GetPatientByHealthRecordID(healthRecordID)))
            {
                return false;
            }

            Appointment appointment = new Appointment
            {
                ID = AppointmentRepository.GetLargestID() + 1,
                Type = AppointmentType.Checkup,
                CreatedDate = DateTime.Now,
                ScheduledDate = scheduleDate,
                Emergency = false,
                DoctorID = doctorID,
                HealthRecordID = healthRecordID,
                HospitalRoomID = hospitalRoomID,
                PatientAnamnesis = null
            };

            AppointmentRepository.Appointments.Add(appointment);
            AppointmentRepository.Save();

            HospitalRoomService.Update(appointment.HospitalRoomID, appointment);
            HospitalRoomRepository.Save();

            return true;
        }

        public static bool Edit(DateTime scheduleDate, DateTime oldScheduleDate, int appointmentID, int doctorID, int patientID, int hospitalRoomID)
        {
            if (PatientService.CheckModificationTroll(PatientService.Get(patientID)))
            {
                return false;
            }

            AppointmentChangeRequest newChangeRequest = new AppointmentChangeRequest()
            {
                ID = AppointmentChangeRequestRepository.GetLargestID() + 1,
                AppointmentID = appointmentID,
                RequestType = RequestType.MakeChanges,
                State = RequestState.Waiting,
                NewDate = scheduleDate,
                NewAppointmentType = AppointmentType.Checkup,
                NewDoctorID = doctorID,
                DateSent = DateTime.Now,
                PatientID = patientID
            };
            if (ShouldSendToSecretary(oldScheduleDate))
            {
                foreach (AppointmentChangeRequest changeRequest in AppointmentChangeRequestRepository.Requests)
                {
                    if (changeRequest.AppointmentID == appointmentID)
                    {
                        changeRequest.State = newChangeRequest.State;
                        changeRequest.NewDate = newChangeRequest.NewDate;
                        changeRequest.NewDoctorID = newChangeRequest.NewDoctorID;
                        changeRequest.RequestType = newChangeRequest.RequestType;
                        AppointmentChangeRequestRepository.Save();
                        return true;
                    }
                }
                AppointmentChangeRequestRepository.Requests.Add(newChangeRequest);
                AppointmentChangeRequestRepository.Save();
                return true;
            }
            else
            {
                HospitalRoomService.AddAppointmentToRoom(hospitalRoomID, newChangeRequest.AppointmentID);
                AppointmentChangeRequestService.EditAppointment(newChangeRequest);
            }

            return true;
        }

        public static bool ShouldSendToSecretary(DateTime scheduleDate)
        {
            TimeSpan timeTillAppointment = scheduleDate.Date.Subtract(DateTime.Now.Date);
            return timeTillAppointment.TotalDays <= 2;
        }

        public static bool Cancel(int appointmentID, int patientID, DateTime appointmentScheduleDate)
        {
            if (PatientService.CheckModificationTroll(PatientService.Get(patientID)))
            {
                return false;
            }

            AppointmentChangeRequest newChangeRequest = new AppointmentChangeRequest
            {
                ID = AppointmentChangeRequestRepository.GetLargestID() + 1,
                AppointmentID = appointmentID,
                RequestType = RequestType.Delete,
                DateSent = DateTime.Now,
                PatientID = patientID
            };

            if (ShouldSendToSecretary(appointmentScheduleDate))
            {
                foreach (AppointmentChangeRequest changeRequest in AppointmentChangeRequestRepository.Requests)
                {
                    if (changeRequest.AppointmentID == newChangeRequest.AppointmentID)
                    {
                        changeRequest.State = RequestState.Waiting;
                        changeRequest.NewDate = newChangeRequest.NewDate;
                        changeRequest.NewDoctorID = newChangeRequest.NewDoctorID;
                        changeRequest.RequestType = newChangeRequest.RequestType;
                        AppointmentChangeRequestRepository.Save();
                        return true;
                    }
                }
                AppointmentChangeRequestRepository.Requests.Add(newChangeRequest);
                AppointmentChangeRequestRepository.Save();
                return true;
            }
            else
            {
                AppointmentChangeRequestService.DeleteAppointment(newChangeRequest);
                AppointmentRepository.Save();
            }

            return true;
        }

        public static List<Appointment> GetByAnamnesisKeyword(string searchKeyword, int healthRecordID)
        {
            List<Appointment> finishedAppointments = GetPatientFinishedAppointments(healthRecordID);
            if (string.IsNullOrEmpty(searchKeyword))
            {
                return finishedAppointments;
            }

            List<Appointment> appointmentsByKeyword = new List<Appointment>();
            foreach (Appointment appointment in finishedAppointments)
            {
                if (appointment.PatientAnamnesis != null &&
                    appointment.PatientAnamnesis.Comment.ToLower().Contains(searchKeyword))
                {
                    appointmentsByKeyword.Add(appointment);
                }
            }

            return appointmentsByKeyword;
        }

        public static bool IsAvailable(DateTime scheduleDate, int doctorID)
        {
            foreach (Appointment appointment in AppointmentRepository.Appointments)
            {
                if (appointment.ScheduledDate.CompareTo(scheduleDate) == 0 && doctorID == appointment.DoctorID)
                {
                    return false;
                }
            }
            return true;
        }

        public static List<Appointment> Sort(List<Appointment> appointments, string sortCriteria)
        {
            switch (sortCriteria)
            {
                case "Date":
                    AppointmentDateCompare appointmentDateComparison = new AppointmentDateCompare();
                    appointments.Sort(appointmentDateComparison);
                    break;
                case "Doctor":
                    AppointmentDoctorCompare appointmentDoctorComparison = new AppointmentDoctorCompare();
                    appointments.Sort(appointmentDoctorComparison);
                    break;
                case "Professional area":
                    AppointmentDoctorCompare appointmentProfessionalAreaComparison = new AppointmentDoctorCompare();
                    appointments.Sort(appointmentProfessionalAreaComparison);
                    break;
            }
            return appointments;
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

        public static Appointment Get(int appointmentID)
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
    }
}
