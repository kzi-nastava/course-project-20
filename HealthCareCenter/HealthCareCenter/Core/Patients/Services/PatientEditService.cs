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

        public PatientEditService(BaseHealthRecordRepository healthRecordRepository)
        {
            _healthRecordRepository = healthRecordRepository;
        }

        public void AddToRepositories(HealthRecord record, Patient patient)
        {
            _healthRecordRepository.Records.Add(record);
            UserRepository.Patients.Add(patient);
            UserRepository.Users.Add(patient);
        }

        public void DeletePatient(Patient patient)
        {
            UserRepository.Patients.Remove(patient);
            UserRepository.Users.Remove(patient);

            if (patient.ID == UserRepository.maxID)
            {
                UserRepository.CalculateMaxID();
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
            UserRepository.SavePatients();
        }
    }
}
