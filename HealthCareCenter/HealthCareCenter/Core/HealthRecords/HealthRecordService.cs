using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Patients;

namespace HealthCareCenter.Core.HealthRecords
{
    public class HealthRecordService : IHealthRecordService
    {
        private readonly BaseHealthRecordRepository _healthRecordRepository;

        public HealthRecordService(BaseHealthRecordRepository healthRecordRepository)
        {
            _healthRecordRepository = healthRecordRepository;
        }

        public HealthRecord GetRecordByPatientID(int id)
        {
            foreach (HealthRecord healthRecord in _healthRecordRepository.Records)
            {
                if (id == healthRecord.PatientID)
                {
                    return healthRecord;
                }
            }
            return null;
        }

        public HealthRecord Get(int id)
        {
            foreach (HealthRecord healthRecord in _healthRecordRepository.Records)
            {
                if (id == healthRecord.ID)
                {
                    return healthRecord;
                }
            }
            return null;
        }

        public HealthRecord Get(Patient patient)
        {
            foreach (HealthRecord record in _healthRecordRepository.Records)
            {
                if (patient.HealthRecordID == record.ID)
                {
                    return record;
                }
            }
            return null;
        }

        public HealthRecord Get(Appointment appointment)
        {
            foreach (HealthRecord record in _healthRecordRepository.Records)
            {
                if (appointment.HealthRecordID == record.ID)
                {
                    return record;
                }
            }
            return null;
        }

        public string CheckAllergens(HealthRecord record)
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
        public string CheckPreviousDiseases(HealthRecord record)
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

        public void Update(double height, double weight, string[] previousDiseases, string[] allergens, int recordIndex)
        {
            HealthRecord healthRecord = _healthRecordRepository.Records[recordIndex];
            healthRecord.Height = height;
            healthRecord.Weight = weight;
            FillPreviousDiseases(previousDiseases, healthRecord);
            FillAllergens(allergens, healthRecord);
        }

        public void FillPreviousDiseases(string[] previousDiseases, HealthRecord record)
        {
            record.PreviousDiseases.Clear();
            foreach (string disease in previousDiseases)
            {
                if (string.IsNullOrWhiteSpace(disease))
                {
                    continue;
                }

                record.PreviousDiseases.Add(disease);
                _healthRecordRepository.Save();
            }
        }
        public void FillAllergens(string[] allergens, HealthRecord record)
        {
            record.Allergens.Clear();
            foreach (string allergen in allergens)
            {
                if (string.IsNullOrWhiteSpace(allergen))
                {
                    continue;
                }

                record.Allergens.Add(allergen);
                _healthRecordRepository.Save();
            }
        }

        public string IsAllergicTo(Medicine.Models.Medicine medicine, HealthRecord record)
        {
            foreach (string ingredient in medicine.Ingredients)
                if (record.Allergens.Contains(ingredient))
                    return ingredient;
            return "";
        }

        public void Delete(Patient patient)
        {
            foreach (HealthRecord record in _healthRecordRepository.Records)
            {
                if (patient.HealthRecordID != record.ID)
                {
                    continue;
                }
                _healthRecordRepository.Records.Remove(record);
                if (patient.HealthRecordID == _healthRecordRepository.LargestID)
                {
                    _healthRecordRepository.CalculateMaxID();
                }
                _healthRecordRepository.Save();
                return;
            }
        }
    }
}
