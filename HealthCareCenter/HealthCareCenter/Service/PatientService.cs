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
    }
}
