using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Model;


namespace HealthCareCenter.Service
{
    public static class PatientService
    {
        public static Patient FindPatient(int ID)
        {
            foreach(Patient patient in UserRepository.Patients)
            {
                if(patient.ID == ID) 
                    return patient; 
            }
            return null; 
        }

        public static int FindPatientIndex(int ID)
        {
            int counter = 0;
            foreach (Patient patient in UserRepository.Patients)
            {
                if (patient.ID == ID)
                    return counter;
                counter++;
            }
            return counter; 
        }
        public static List<Patient> GetBlockedPatients()
        {
            List<Patient> blockedPatients = new List<Patient>();
            foreach (Patient patient in UserRepository.Patients)
            {
                if (patient.IsBlocked)
                {
                    blockedPatients.Add(patient);
                }
            }
            return blockedPatients;
        }

        public static void Block(Patient patient)
        {
            patient.IsBlocked = true;
            patient.BlockedBy = Enums.Blocker.Secretary;
            UserRepository.SavePatients();
        }

        public static void Block(Patient patient, List<Patient> blockedPatients)
        {
            Block(patient);
            blockedPatients.Add(patient);
        }

        public static void Unblock(Patient patient)
        {
            patient.IsBlocked = false;
            patient.BlockedBy = Enums.Blocker.None;
            UserRepository.SavePatients();
        }

        public static void Unblock(Patient patient, List<Patient> blockedPatients)
        {
            Unblock(patient);
            blockedPatients.Remove(patient);
        }

        public static void UpdateMaxIDsIfNeeded()
        {
            if (UserRepository.maxID == -1)
            {
                UserRepository.CalculateMaxID();
            }
            if (HealthRecordRepository.maxID == -1)
            {
                HealthRecordRepository.CalculateMaxID();
            }
        }

        public static void Delete(Patient patient)
        {
            HealthRecordService.Delete(patient);
            HealthRecordRepository.Save();

            DeletePatient(patient);
            UserRepository.SavePatients();
        }

        private static void DeletePatient(Patient patient)
        {
            UserRepository.Patients.Remove(patient);
            UserRepository.Users.Remove(patient);

            if (patient.ID == UserRepository.maxID)
            {
                UserRepository.CalculateMaxID();
            }
        }

        public static void Create(Patient patient, HealthRecord record)
        {
            HealthRecordRepository.maxID++;
            UserRepository.maxID++;

            AddToRepositories(record, patient);
            SaveRepositories();
        }

        public static void Edit(Patient uneditedPatient, HealthRecord uneditedRecord, Patient editedPatient, HealthRecord editedRecord)
        {
            EditHealthRecord(uneditedRecord, editedRecord);
            EditPatient(uneditedPatient, editedPatient);
            SaveRepositories();
        }

        private static void EditPatient(Patient uneditedPatient, Patient editedPatient)
        {
            uneditedPatient.Username = editedPatient.Username;
            uneditedPatient.Password = editedPatient.Password;
            uneditedPatient.FirstName = editedPatient.FirstName;
            uneditedPatient.LastName = editedPatient.LastName;
            uneditedPatient.DateOfBirth = editedPatient.DateOfBirth;
        }

        private static void EditHealthRecord(HealthRecord uneditedRecord, HealthRecord editedRecord)
        {
            uneditedRecord.Height = editedRecord.Height;
            uneditedRecord.Weight = editedRecord.Weight;
            uneditedRecord.PreviousDiseases = editedRecord.PreviousDiseases;
            uneditedRecord.Allergens = editedRecord.Allergens;
        }

        private static void SaveRepositories()
        {
            HealthRecordRepository.Save();
            UserRepository.SavePatients();
        }

        private static void AddToRepositories(HealthRecord record, Patient patient)
        {
            HealthRecordRepository.Records.Add(record);
            UserRepository.Patients.Add(patient);
            UserRepository.Users.Add(patient);
        }
    }
}
