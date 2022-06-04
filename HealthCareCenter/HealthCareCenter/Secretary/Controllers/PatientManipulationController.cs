using HealthCareCenter.Model;
using HealthCareCenter.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Secretary.Controllers
{
    public class PatientManipulationController
    {
        public PatientManipulationController()
        {
            HealthRecordRepository.Load();
            VacationRequestRepository.Load();
        }

        public List<Patient> GetBlockedPatients()
        {
            return PatientService.GetBlockedPatients();
        }

        public void Block(Patient patient, List<Patient> blockedPatients)
        {
            if (!patient.IsBlocked)
            {
                PatientService.Block(patient, blockedPatients);
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
                PatientService.Unblock(patient, blockedPatients);
            }
            else
            {
                throw new Exception("Patient is not blocked.");
            }
        }

        public void UpdateMaxIDsIfNeeded()
        {
            PatientService.UpdateMaxIDsIfNeeded();
        }

        public void Delete(Patient patient)
        {
            PatientService.Delete(patient);
        }

        public HealthRecord GetRecord(Patient patient)
        {
            return HealthRecordService.Get(patient);
        }
    }
}
