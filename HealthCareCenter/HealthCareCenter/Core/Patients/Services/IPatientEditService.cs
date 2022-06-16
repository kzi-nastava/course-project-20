using HealthCareCenter.Core.HealthRecords;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Patients.Services
{
    public interface IPatientEditService
    {
        void DeletePatient(Patient patient);
        void EditPatient(Patient uneditedPatient, Patient editedPatient);
        void EditHealthRecord(HealthRecord uneditedRecord, HealthRecord editedRecord);
        void SaveRepositories();
        void AddToRepositories(HealthRecord record, Patient patient);
    }
}
