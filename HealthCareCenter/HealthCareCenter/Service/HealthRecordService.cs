using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Model;

namespace HealthCareCenter.Service
{
    public static class HealthRecordService
    {
        public static int maxID = -1;

        public static void CalculateMaxID()
        {
            maxID = -1;
            foreach (HealthRecord record in HealthRecordRepository.Records)
            {
                if (record.ID > maxID)
                {
                    maxID = record.ID;
                }
            }
        }

        public static HealthRecord FindRecordByPatientID(int ID)
        {
            foreach (HealthRecord healthRecord in HealthRecordRepository.Records)
            {
                if (ID == healthRecord.PatientID)
                {
                    return healthRecord;
                }
            }
            return null;
        }
        public static HealthRecord FindRecord(int ID)
        {
            foreach (HealthRecord healthRecord in HealthRecordRepository.Records)
            {
                if (ID == healthRecord.ID)
                {
                    return healthRecord;
                }
            }
            return null;
        }
        public static HealthRecord FindRecord(Patient patient)
        {
            foreach (HealthRecord record in HealthRecordRepository.Records)
            {
                if (patient.HealthRecordID == record.ID)
                {
                    return record;
                }
            }
            return null;
        }

        public static HealthRecord Find(Appointment appointment)
        {
            foreach (HealthRecord record in HealthRecordRepository.Records)
            {
                if (appointment.HealthRecordID == record.ID)
                {
                    return record;
                }
            }
            return null;
        }
        public static string CheckAlergens(HealthRecord healthRecord)
        {
            string alergens = "";
            if (healthRecord.Allergens != null)
            {
                foreach (string s in healthRecord.Allergens)
                {
                    alergens += "," + s;
                }

                return alergens;
            }
            else
            {
                return "none";
            }
        }
        public static string CheckPreviousDiseases(HealthRecord healthRecord)
        {
            string previousDiseases = "";
            if (healthRecord.PreviousDiseases != null)
            {
                foreach (string s in healthRecord.PreviousDiseases)
                {
                    previousDiseases += "," + s;
                }

                return previousDiseases;
            }
            else
            {
                return "none";
            }
        }

        public static void UpdateHealthRecord(double height, double weight, string[] previousDiseases, string[] alergens, int healthRecordIndex)
        {
            HealthRecord healthRecord = HealthRecordRepository.Records[healthRecordIndex];
            healthRecord.Height = height;
            healthRecord.Weight = weight;
            FillPreviousDiseases(previousDiseases, healthRecord);
            FillAlergens(alergens, healthRecord);


        }

        public static void FillPreviousDiseases(string[] previousDiseases, HealthRecord healthRecord)
        {
            healthRecord.PreviousDiseases.Clear();
            foreach (string disease in previousDiseases)
            {
                if (string.IsNullOrWhiteSpace(disease))
                {
                    continue;
                }

                healthRecord.PreviousDiseases.Add(disease);
            }
        }
        public static void FillAlergens(string[] allergens, HealthRecord healthRecord)
        {
            healthRecord.Allergens.Clear();
            foreach (string allergen in allergens)
            {
                if (string.IsNullOrWhiteSpace(allergen))
                {
                    continue;
                }

                healthRecord.Allergens.Add(allergen);
            }
        }

        public static string IsAllergicTo(Medicine medicine,HealthRecord healthRecord)
        {
            foreach (string ingredient in medicine.Ingredients)
                if (healthRecord.Allergens.Contains(ingredient))
                    return ingredient;
            return "";
        }
        
    }
}
