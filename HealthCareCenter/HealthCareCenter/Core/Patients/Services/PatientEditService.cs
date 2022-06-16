using HealthCareCenter.Core.HealthRecords;
using HealthCareCenter.Core.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Patients.Services
{
    public class PatientEditService : IPatientEditService
    {
        private readonly BaseHealthRecordRepository _healthRecordRepository;
        private readonly BaseUserRepository _userRepository;

        public PatientEditService(
            BaseHealthRecordRepository healthRecordRepository,
            BaseUserRepository userRepository)
        {
            _healthRecordRepository = healthRecordRepository;
            _userRepository = userRepository;
        }

        public void AddToRepositories(HealthRecord record, Patient patient)
        {
            _healthRecordRepository.Records.Add(record);
            _userRepository.Patients.Add(patient);
            _userRepository.Users.Add(patient);
        }

        public void DeletePatient(Patient patient)
        {
            _userRepository.Patients.Remove(patient);
            _userRepository.Users.Remove(patient);

            if (patient.ID == _userRepository.LargestID)
            {
                _userRepository.CalculateMaxID();
            }
        }

        public void EditHealthRecord(HealthRecord uneditedRecord, HealthRecord editedRecord)
        {
            uneditedRecord.Height = editedRecord.Height;
            uneditedRecord.Weight = editedRecord.Weight;
            uneditedRecord.PreviousDiseases = editedRecord.PreviousDiseases;
            uneditedRecord.Allergens = editedRecord.Allergens;
        }

        public void EditPatient(Patient uneditedPatient, Patient editedPatient)
        {
            uneditedPatient.Username = editedPatient.Username;
            uneditedPatient.Password = editedPatient.Password;
            uneditedPatient.FirstName = editedPatient.FirstName;
            uneditedPatient.LastName = editedPatient.LastName;
            uneditedPatient.DateOfBirth = editedPatient.DateOfBirth;
        }

        public void SaveRepositories()
        {
            _healthRecordRepository.Save();
            _userRepository.SavePatients();
        }
    }
}
