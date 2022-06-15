using HealthCareCenter.Core.HealthRecords;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Patients.Services
{
    public interface IPatientService
    {
        Patient Get(int id);
        Patient GetPatientByHealthRecordID(int healthRecordID);
        int GetIndex(int id);
        List<Patient> GetBlockedPatients();
        void Block(Patient patient);
        void Block(Patient patient, List<Patient> blockedPatients);
        void Unblock(Patient patient);
        void Unblock(Patient patient, List<Patient> blockedPatients);
        void UpdateMaxIDsIfNeeded();
        void Delete(Patient patient);
        void Create(Patient patient, HealthRecord record);
        void Edit(Patient uneditedPatient, HealthRecord uneditedRecord, Patient editedPatient, HealthRecord editedRecord);
        bool CheckCreationTroll(Patient possibleTroll);
        bool CheckModificationTroll(Patient possibleTroll);
    }
}
