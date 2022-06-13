using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Patients.Models;

namespace HealthCareCenter.Core.HealthRecords
{
    public static class HealthRecordService
    {
        public static HealthRecord GetRecordByPatientID(int id)
        {
            foreach (HealthRecord healthRecord in HealthRecordRepository.Records)
            {
                if (id == healthRecord.PatientID)
                {
                    return healthRecord;
                }
            }
            return null;
        }

        public static HealthRecord Get(int id)
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

        public static HealthRecord Get(Patient patient)
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

        public static HealthRecord Get(Appointment appointment)
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

        public static string CheckAllergens(HealthRecord record)
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

        public static void Update(double height, double weight, string[] previousDiseases, string[] allergens, int recordIndex)
        {
            HealthRecord healthRecord = HealthRecordRepository.Records[recordIndex];
            healthRecord.Height = height;
            healthRecord.Weight = weight;
            FillPreviousDiseases(previousDiseases, healthRecord);
            FillAllergens(allergens, healthRecord);
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
        public static void FillAllergens(string[] allergens, HealthRecord record)
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

        public static string IsAllergicTo(Medicine.Models.Medicine medicine, HealthRecord record)
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
