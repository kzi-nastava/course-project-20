using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Appointments.Repository;
using HealthCareCenter.Core.Appointments.Urgent;
using HealthCareCenter.Core.Patients;
using HealthCareCenter.Core.Patients.Services;
using HealthCareCenter.Core.Rooms.Repositories;
using HealthCareCenter.Core.Rooms.Services;
using HealthCareCenter.Core.Users;
using System;
using System.Collections.Generic;

namespace HealthCareCenter.Core.Appointments.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly BaseAppointmentRepository _appointmentRepository;
        private readonly BaseAppointmentChangeRequestRepository _changeRequestRepository;
        private readonly IAppointmentChangeRequestService _changeRequestService;
        private readonly IPatientService _patientService;
        private readonly IHospitalRoomService _hospitalRoomService;
        private readonly BaseHospitalRoomRepository _hospitalRoomRepository;

        public AppointmentService(
            BaseAppointmentRepository appointmentRepository,
            BaseAppointmentChangeRequestRepository changeRequestRepository,
            IAppointmentChangeRequestService changeRequestService,
            IPatientService patientService,
            IHospitalRoomService hospitalRoomService,
            BaseHospitalRoomRepository hospitalRoomRepository)
        {
            _appointmentRepository = appointmentRepository;
            _changeRequestRepository = changeRequestRepository;
            _changeRequestService = changeRequestService;
            _patientService = patientService;
            _hospitalRoomService = hospitalRoomService;
            _hospitalRoomRepository = hospitalRoomRepository;
        }

        public Appointment Get(OccupiedAppointment appointmentDisplay)
        {
            foreach (Appointment appointment in _appointmentRepository.Appointments)
            {
                if (appointment.ID == appointmentDisplay.ID)
                {
                    return appointment;
                }
            }
            return null;
        }

        public List<Appointment> GetPatientUnfinishedAppointments(int patientHealthRecordID)
        {
            List<Appointment> unfinishedAppointments = new List<Appointment>();
            foreach (Appointment potentialAppointment in _appointmentRepository.Appointments)
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

        public List<Appointment> GetPatientFinishedAppointments(int patientHealthRecordID)
        {
            List<Appointment> finishedAppointments = new List<Appointment>();
            foreach (Appointment potentialAppointment in _appointmentRepository.Appointments)
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

        public bool Schedule(Appointment appointment, bool checkTroll)
        {
            if (checkTroll && _patientService.CheckCreationTroll(_patientService.GetPatientByHealthRecordID(appointment.HealthRecordID)))
            {
                return false;
            }

            _appointmentRepository.Appointments.Add(appointment);
            _appointmentRepository.Save();

            _hospitalRoomService.Update(appointment.HospitalRoomID, appointment);
            _hospitalRoomRepository.Save();

            return true;
        }

        public bool Schedule(DateTime scheduleDate, int doctorID, int healthRecordID, int hospitalRoomID)
        {
            if (_patientService.CheckCreationTroll(_patientService.GetPatientByHealthRecordID(healthRecordID)))
            {
                return false;
            }

            Appointment appointment = new Appointment
            {
                ID = _appointmentRepository.GetLargestID() + 1,
                Type = AppointmentType.Checkup,
                CreatedDate = DateTime.Now,
                ScheduledDate = scheduleDate,
                Emergency = false,
                DoctorID = doctorID,
                HealthRecordID = healthRecordID,
                HospitalRoomID = hospitalRoomID,
                PatientAnamnesis = null
            };

            _appointmentRepository.Appointments.Add(appointment);
            _appointmentRepository.Save();

            _hospitalRoomService.Update(appointment.HospitalRoomID, appointment);
            _hospitalRoomRepository.Save();

            return true;
        }

        public bool Edit(DateTime scheduleDate, DateTime oldScheduleDate, int appointmentID, int doctorID, int patientID, int hospitalRoomID)
        {
            if (_patientService.CheckModificationTroll(_patientService.Get(patientID)))
            {
                return false;
            }

            AppointmentChangeRequest newChangeRequest = new AppointmentChangeRequest()
            {
                ID = _appointmentRepository.GetLargestID() + 1,
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
                foreach (AppointmentChangeRequest changeRequest in _changeRequestRepository.Requests)
                {
                    if (changeRequest.AppointmentID == appointmentID)
                    {
                        changeRequest.State = newChangeRequest.State;
                        changeRequest.NewDate = newChangeRequest.NewDate;
                        changeRequest.NewDoctorID = newChangeRequest.NewDoctorID;
                        changeRequest.RequestType = newChangeRequest.RequestType;
                        _changeRequestRepository.Save();
                        return true;
                    }
                }
                _changeRequestRepository.Requests.Add(newChangeRequest);
                _changeRequestRepository.Save();
                return true;
            }
            else
            {
                _hospitalRoomService.AddAppointmentToRoom(hospitalRoomID, newChangeRequest.AppointmentID);
                _changeRequestService.EditAppointment(newChangeRequest);
            }

            return true;
        }

        public bool ShouldSendToSecretary(DateTime scheduleDate)
        {
            TimeSpan timeTillAppointment = scheduleDate.Date.Subtract(DateTime.Now.Date);
            return timeTillAppointment.TotalDays <= 2;
        }

        public bool Cancel(int appointmentID, int patientID, DateTime appointmentScheduleDate)
        {
            if (_patientService.CheckModificationTroll(_patientService.Get(patientID)))
            {
                return false;
            }

            AppointmentChangeRequest newChangeRequest = new AppointmentChangeRequest
            {
                ID = _changeRequestRepository.GetLargestID() + 1,
                AppointmentID = appointmentID,
                RequestType = RequestType.Delete,
                DateSent = DateTime.Now,
                PatientID = patientID
            };

            if (ShouldSendToSecretary(appointmentScheduleDate))
            {
                foreach (AppointmentChangeRequest changeRequest in _changeRequestRepository.Requests)
                {
                    if (changeRequest.AppointmentID == newChangeRequest.AppointmentID)
                    {
                        changeRequest.State = RequestState.Waiting;
                        changeRequest.NewDate = newChangeRequest.NewDate;
                        changeRequest.NewDoctorID = newChangeRequest.NewDoctorID;
                        changeRequest.RequestType = newChangeRequest.RequestType;
                        _changeRequestRepository.Save();
                        return true;
                    }
                }
                _changeRequestRepository.Requests.Add(newChangeRequest);
                _changeRequestRepository.Save();
                return true;
            }
            else
            {
                _changeRequestService.DeleteAppointment(newChangeRequest);
                _changeRequestRepository.Save();
            }

            return true;
        }

        public List<Appointment> GetByAnamnesisKeyword(string searchKeyword, int healthRecordID)
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

        public bool IsAvailable(DateTime scheduleDate, int doctorID)
        {
            foreach (Appointment appointment in _appointmentRepository.Appointments)
            {
                if (appointment.ScheduledDate.CompareTo(scheduleDate) == 0 && doctorID == appointment.DoctorID)
                {
                    return false;
                }
            }
            return true;
        }

        public List<Appointment> Sort(List<Appointment> appointments, string sortCriteria)
        {
            switch (sortCriteria)
            {
                case "Date":
                    AppointmentDateCompare appointmentDateComparison = new AppointmentDateCompare();
                    appointments.Sort(appointmentDateComparison);
                    break;
                case "Doctor":
                    AppointmentDoctorCompare appointmentDoctorComparison = new AppointmentDoctorCompare(new UserRepository());
                    appointments.Sort(appointmentDoctorComparison);
                    break;
                case "Professional area":
                    AppointmentDoctorCompare appointmentProfessionalAreaComparison = new AppointmentDoctorCompare(new UserRepository());
                    appointments.Sort(appointmentProfessionalAreaComparison);
                    break;
            }
            return appointments;
        }

        public List<Appointment> GetAppointmentsInTheFollowingDays(DateTime date, int numberOfDays)
        {
            List<Appointment> appointments = new List<Appointment>();
            foreach (Appointment appointment in _appointmentRepository.Appointments)
            {
                TimeSpan timeSpan = appointment.ScheduledDate.Subtract(date);
                if (timeSpan.TotalDays <= 3 && timeSpan.TotalDays >= 0)
                {
                    appointments.Add(appointment);
                }
            }
            return appointments;
        }

        public Appointment Get(int appointmentID)
        {
            foreach (Appointment appointment in _appointmentRepository.Appointments)
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
