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

        public static HealthRecord FindRecord(Appointment appointment)
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
    }
}
