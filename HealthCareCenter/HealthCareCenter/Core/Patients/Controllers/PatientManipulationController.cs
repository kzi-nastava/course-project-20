using HealthCareCenter.Core.HealthRecords;
using HealthCareCenter.Core.Patients.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Patients.Controllers
{
    public class PatientManipulationController
    {
        private readonly IHealthRecordService _healthRecordService;
        private readonly IPatientService _patientService;

        public PatientManipulationController(
            IHealthRecordService healthRecordService,
            IPatientService patientService)
        {
            _healthRecordService = healthRecordService;
            _patientService = patientService;
        }

        public List<Patient> GetBlockedPatients()
        {
            return _patientService.GetBlockedPatients();
        }

        public void Block(Patient patient, List<Patient> blockedPatients)
        {
            if (!patient.IsBlocked)
            {
                _patientService.Block(patient, blockedPatients);
            }
            else
            {
                throw new Exception("Patient is already blocked.");
            }
        }

        public void Unblock(Patient patient, List<Patient> blockedPatients)
        {
            if (patient.IsBlocked)
            {
                _patientService.Unblock(patient, blockedPatients);
            }
            else
            {
                throw new Exception("Patient is not blocked.");
            }
        }

        public void UpdateMaxIDsIfNeeded()
        {
            _patientService.UpdateMaxIDsIfNeeded();
        }

        public void Delete(Patient patient)
        {
            _patientService.Delete(patient);
        }

        public HealthRecord GetRecord(Patient patient)
        {
            return _healthRecordService.Get(patient);
        }
    }
}
