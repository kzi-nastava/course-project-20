using HealthCareCenter.Secretary.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Secretary.Controllers
{
    public class PatientController
    {
        public string ValidatePreviousDisease(string disease)
        {
            if (string.IsNullOrWhiteSpace(disease))
            {
                throw new Exception("You must enter a disease.");
            }

            return disease;
        }

        public string ValidateAllergen(string allergen)
        {
            if (string.IsNullOrWhiteSpace(allergen))
            {
                throw new Exception("You must enter an allergen.");
            }

            return allergen;
        }

        protected void CheckIfDataEntered(PatientDTO patient, HealthRecordDTO record)
        {
            if (string.IsNullOrWhiteSpace(patient.FirstName))
            {
                throw new Exception("You must enter a first name.");
            }
            if (string.IsNullOrWhiteSpace(patient.Username))
            {
                throw new Exception("You must enter a username.");
            }
            if (string.IsNullOrWhiteSpace(patient.LastName))
            {
                throw new Exception("You must enter a last name.");
            }
            if (string.IsNullOrWhiteSpace(patient.Password))
            {
                throw new Exception("You must enter a password.");
            }
            if (string.IsNullOrWhiteSpace(record.Height))
            {
                throw new Exception("You must enter a height.");
            }
            if (string.IsNullOrWhiteSpace(record.Weight))
            {
                throw new Exception("You must enter a weight.");
            }
            if (patient.DateOfBirth == null)
            {
                throw new Exception("You must enter a date of birth.");
            }
        }

        protected void ValidateBirthDate(PatientDTO patient)
        {
            if (patient.DateOfBirth > DateTime.Now)
            {
                throw new Exception("You cannot enter a date in the future.");
            }
        }

        protected void ValidateHeight(HealthRecordDTO record)
        {
            if (!double.TryParse(record.Height, out _))
            {
                throw new Exception("Height must be a number.");
            }
        }

        protected void ValidateWeight(HealthRecordDTO record)
        {
            if (!double.TryParse(record.Weight, out _))
            {
                throw new Exception("Weight must be a number.");
            }
        }
    }
}
