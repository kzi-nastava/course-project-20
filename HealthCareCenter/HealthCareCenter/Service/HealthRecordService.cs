using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Model;

namespace HealthCareCenter.Service
{
    public static class HealthRecordService
    {
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

        public static HealthRecord Find(int id)
        {
            foreach (HealthRecord healthRecord in HealthRecordRepository.Records)
            {
                if (id == healthRecord.ID)
                {
                    return healthRecord;
                }
            }
            return null;
        }

        public static HealthRecord Find(Patient patient)
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
            if (HealthRecordRepository.Records == null)
            {
                HealthRecordRepository.Load();
            }

            foreach (HealthRecord record in HealthRecordRepository.Records)
            {
                if (appointment.HealthRecordID == record.ID)
                {
                    return record;
                }
            }
            return null;
        }

        public static string CheckAlergens(HealthRecord record)
        {
            string alergens = "";
            if (record.Allergens != null)
            {
                foreach (string s in record.Allergens)
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
        public static string CheckPreviousDiseases(HealthRecord record)
        {
            string previousDiseases = "";
            if (record.PreviousDiseases != null)
            {
                foreach (string s in record.PreviousDiseases)
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

        public static void UpdateHealthRecord(double height, double weight, string[] previousDiseases, string[] allergens, int recordIndex)
        {
            HealthRecord healthRecord = HealthRecordRepository.Records[recordIndex];
            healthRecord.Height = height;
            healthRecord.Weight = weight;
            FillPreviousDiseases(previousDiseases, healthRecord);
            FillAlergens(allergens, healthRecord);
        }

        public static void FillPreviousDiseases(string[] previousDiseases, HealthRecord record)
        {
            record.PreviousDiseases.Clear();
            foreach (string disease in previousDiseases)
            {
                if (string.IsNullOrWhiteSpace(disease))
                {
                    continue;
                }

                record.PreviousDiseases.Add(disease);
            }
        }
        public static void FillAlergens(string[] allergens, HealthRecord record)
        {
            record.Allergens.Clear();
            foreach (string allergen in allergens)
            {
                if (string.IsNullOrWhiteSpace(allergen))
                {
                    continue;
                }

                record.Allergens.Add(allergen);
            }
        }

        public static string IsAllergicTo(Medicine medicine, HealthRecord record)
        {
            foreach (string ingredient in medicine.Ingredients)
                if (record.Allergens.Contains(ingredient))
                    return ingredient;
            return "";
        }
        
        public static void Delete(Patient patient)
        {
            foreach (HealthRecord record in HealthRecordRepository.Records)
            {
                if (patient.HealthRecordID != record.ID)
                {
                    continue;
                }
                HealthRecordRepository.Records.Remove(record);
                if (patient.HealthRecordID == HealthRecordRepository.maxID)
                {
                    HealthRecordRepository.CalculateMaxID();
                }
                return;
            }
        }
    }
}
