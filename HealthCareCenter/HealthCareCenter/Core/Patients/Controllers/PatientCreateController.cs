using HealthCareCenter.Core.HealthRecords;
using HealthCareCenter.Core.Patients.Services;
using HealthCareCenter.Core.Users;
using HealthCareCenter.Core.Users.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Patients.Controllers
{
    public class PatientCreateController : PatientController
    {
        private readonly IPatientService _patientService;
        private readonly BaseUserRepository _userRepository;

        public PatientCreateController(IPatientService patientService, BaseUserRepository userRepository)
        {
            _patientService = patientService;
            _userRepository = userRepository;
        }

        public void Create(PatientDTO patientDTO, HealthRecordDTO recordDTO)
        {
            try
            {
                CheckIfDataEntered(patientDTO, recordDTO);
                ValidateData(patientDTO, recordDTO);
            }
            catch (Exception)
            {
                throw;
            }

            double height = double.Parse(recordDTO.Height);
            double weight = double.Parse(recordDTO.Weight);

            HealthRecord record = new HealthRecord(recordDTO.ID, height, weight, recordDTO.PreviousDiseases, recordDTO.Allergens, recordDTO.PatientID);
            Patient patient = new Patient(patientDTO.ID, patientDTO.Username, patientDTO.Password, patientDTO.FirstName, patientDTO.LastName, (DateTime)patientDTO.DateOfBirth, patientDTO.IsBlocked, patientDTO.BlockedBy, patientDTO.PrescriptionIDs, patientDTO.HealthRecordID);

            _patientService.Create(patient, record);
        }

        private void ValidateUsername(PatientDTO patient)
        {
            foreach (User user in _userRepository.Users)
            {
                if (user.Username == patient.Username)
                {
                    throw new Exception("Username is already in use. Choose a different one.");
                }
            }
        }

        private void ValidateData(PatientDTO patient, HealthRecordDTO record)
        {
            try
            {
                ValidateBirthDate(patient);
                ValidateUsername(patient);
                ValidateHeight(record);
                ValidateWeight(record);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
