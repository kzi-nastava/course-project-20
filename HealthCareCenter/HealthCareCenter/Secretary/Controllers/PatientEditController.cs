using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Model;
using HealthCareCenter.Secretary.DTOs;
using HealthCareCenter.Service;

namespace HealthCareCenter.Secretary.Controllers
{
    public class PatientEditController : PatientController
    {
        public void Edit(PatientDTO editedPatientDTO, HealthRecordDTO editedRecordDTO, Patient uneditedPatient, HealthRecord uneditedRecord)
        {
            try
            {
                CheckIfDataEntered(editedPatientDTO, editedRecordDTO);
                ValidateData(editedPatientDTO, editedRecordDTO, uneditedPatient);
            }
            catch (Exception)
            {
                throw;
            }

            double height = Double.Parse(editedRecordDTO.Height);
            double weight = Double.Parse(editedRecordDTO.Weight);

            HealthRecord editedRecord = new HealthRecord(editedRecordDTO.ID, height, weight, editedRecordDTO.PreviousDiseases, editedRecordDTO.Allergens, editedRecordDTO.PatientID);
            Patient editedPatient = new Patient(editedPatientDTO.ID, editedPatientDTO.Username, editedPatientDTO.Password, editedPatientDTO.FirstName, editedPatientDTO.LastName, (DateTime)editedPatientDTO.DateOfBirth, editedPatientDTO.IsBlocked, editedPatientDTO.BlockedBy, editedPatientDTO.PrescriptionIDs, editedPatientDTO.HealthRecordID);

            PatientService.Edit(uneditedPatient, uneditedRecord, editedPatient, editedRecord);
        }

        private void ValidateData(PatientDTO patient, HealthRecordDTO record, Patient uneditedPatient)
        {
            try
            {
                ValidateBirthDate(patient);
                ValidateUsername(patient, uneditedPatient);
                ValidateHeight(record);
                ValidateWeight(record);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ValidateUsername(PatientDTO editedPatient, Patient uneditedPatient)
        {
            if (uneditedPatient.Username == editedPatient.Username)
            {
                return;
            }
            foreach (User user in UserRepository.Users)
            {
                if (user.Username == editedPatient.Username)
                {
                    throw new Exception("Username is already in use. Choose a different one.");
                }
            }
        }
    }
}
