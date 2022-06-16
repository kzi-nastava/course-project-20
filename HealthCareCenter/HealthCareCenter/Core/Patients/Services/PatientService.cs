using System;
using System.Collections.Generic;
using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Appointments.Repository;
using HealthCareCenter.Core.HealthRecords;
using HealthCareCenter.Core.Users;


namespace HealthCareCenter.Core.Patients.Services
{
    public class PatientService : IPatientService
    {
        private const int _creationTrollLimit = 100;
        private const int _modificationTrollLimit = 100;

        // change to parametarized constructor when PatientService refactoring starts
        private readonly BaseAppointmentRepository _appointmentRepository;
        private readonly BaseAppointmentChangeRequestRepository _changeRequestRepository;
        private readonly BaseHealthRecordRepository _healthRecordRepository;
        private readonly IHealthRecordService _healthRecordService;
        private readonly IPatientEditService _patientEditService;
        private readonly BaseUserRepository _userRepository;

        public PatientService(
            BaseAppointmentRepository appointmentRepository,
            BaseAppointmentChangeRequestRepository changeRequestRepository,
            BaseHealthRecordRepository healthRecordRepository,
            IHealthRecordService healthRecordService,
            IPatientEditService patientEditService,
            BaseUserRepository userRepository)
        {
            _appointmentRepository = appointmentRepository;
            _changeRequestRepository = changeRequestRepository;
            _healthRecordRepository = healthRecordRepository;
            _healthRecordService= healthRecordService;
            _patientEditService = patientEditService;
            _userRepository = userRepository;
        }

        public Patient Get(int id)
        {
            foreach (Patient patient in _userRepository.Patients)
            {
                if (patient.ID == id)
                    return patient;
            }
            return null;
        }

        public Patient GetPatientByHealthRecordID(int healthRecordID)
        {
            foreach (Patient patient in _userRepository.Patients)
            {
                if (patient.HealthRecordID == healthRecordID)
                {
                    return patient;
                }
            }
            return null;
        }

        public int GetIndex(int id)
        {
            int counter = 0;
            foreach (Patient patient in _userRepository.Patients)
            {
                if (patient.ID == id)
                    return counter;
                counter++;
            }
            return counter;
        }

        public List<Patient> GetBlockedPatients()
        {
            List<Patient> blockedPatients = new List<Patient>();
            foreach (Patient patient in _userRepository.Patients)
            {
                if (patient.IsBlocked)
                {
                    blockedPatients.Add(patient);
                }
            }
            return blockedPatients;
        }

        public void Block(Patient patient)
        {
            patient.IsBlocked = true;
            patient.BlockedBy = Blocker.Secretary;
            _userRepository.SavePatients();
        }

        public void Block(Patient patient, List<Patient> blockedPatients)
        {
            Block(patient);
            blockedPatients.Add(patient);
        }

        public void Unblock(Patient patient)
        {
            patient.IsBlocked = false;
            patient.BlockedBy = Blocker.None;
            _userRepository.SavePatients();
        }

        public void Unblock(Patient patient, List<Patient> blockedPatients)
        {
            Unblock(patient);
            blockedPatients.Remove(patient);
        }

        public void UpdateMaxIDsIfNeeded()
        {
            if (_userRepository.LargestID == -1)
            {
                _userRepository.CalculateMaxID();
            }
            if (_healthRecordRepository.LargestID == -1)
            {
                _healthRecordRepository.CalculateMaxID();
            }
        }

        public void Delete(Patient patient)
        {
            _healthRecordService.Delete(patient);
            _healthRecordRepository.Save();

            _patientEditService.DeletePatient(patient);
            _userRepository.SavePatients();
        }

        public void Create(Patient patient, HealthRecord record)
        {
            _healthRecordRepository.LargestID++;
            _userRepository.LargestID++;

            _patientEditService.AddToRepositories(record, patient);
            _patientEditService.SaveRepositories();
        }

        public void Edit(Patient uneditedPatient, HealthRecord uneditedRecord, Patient editedPatient, HealthRecord editedRecord)
        {
            _patientEditService.EditHealthRecord(uneditedRecord, editedRecord);
            _patientEditService.EditPatient(uneditedPatient, editedPatient);
            _patientEditService.SaveRepositories();
        }

        public bool CheckCreationTroll(Patient possibleTroll)
        {
            int creationCount = 0;
            foreach (Appointment appointment in _appointmentRepository.Appointments)
            {
                if (appointment.HealthRecordID == possibleTroll.HealthRecordID)
                {
                    TimeSpan timePassedSinceScheduling = DateTime.Now.Subtract(appointment.CreatedDate);
                    if (timePassedSinceScheduling.TotalDays < 30)
                    {
                        ++creationCount;
                    }
                }
            }

            if (creationCount >= _creationTrollLimit)
            {
                foreach (Patient patient in _userRepository.Patients)
                {
                    if (possibleTroll.ID == patient.ID)
                    {
                        patient.IsBlocked = true;
                        patient.BlockedBy = Blocker.System;
                        break;
                    }

                }
                return true;
            }

            return false;
        }

        public bool CheckModificationTroll(Patient possibleTroll)
        {
            int modificationCount = 0;
            foreach (AppointmentChangeRequest changeRequest in _changeRequestRepository.Requests)
            {
                if (changeRequest.PatientID == possibleTroll.ID)
                {
                    TimeSpan timePassedSinceScheduling = DateTime.Now.Subtract(changeRequest.DateSent);
                    if (timePassedSinceScheduling.TotalDays < 30)
                    {
                        ++modificationCount;
                    }
                }
            }

            if (modificationCount >= _modificationTrollLimit)
            {
                foreach (Patient patient in _userRepository.Patients)
                {
                    if (possibleTroll.ID == patient.ID)
                    {
                        patient.IsBlocked = true;
                        patient.BlockedBy = Blocker.System;
                        break;
                    }
                }

                return true;
            }

            return false;
        }
    }
}
