using HealthCareCenter.Core.Patients.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Patients.Controllers
{
    public class PatientViewController
    {
        private readonly IPatientService _patientService;

        public PatientViewController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        public void Block(Patient patient)
        {
            if (patient.IsBlocked)
            {
                throw new Exception("Patient is already blocked.");
            }
            _patientService.Block(patient);
        }

        public void Unblock(Patient patient)
        {
            if (!patient.IsBlocked)
            {
                throw new Exception("Patient is not blocked.");
            }
            _patientService.Unblock(patient);
        }
    }
}
