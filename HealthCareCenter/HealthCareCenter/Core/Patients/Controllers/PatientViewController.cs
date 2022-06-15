﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Patients.Controllers
{
    public class PatientViewController
    {
        public void Block(Patient patient)
        {
            if (patient.IsBlocked)
            {
                throw new Exception("Patient is already blocked.");
            }
            PatientService.Block(patient);
        }

        public void Unblock(Patient patient)
        {
            if (!patient.IsBlocked)
            {
                throw new Exception("Patient is not blocked.");
            }
            PatientService.Unblock(patient);
        }
    }
}