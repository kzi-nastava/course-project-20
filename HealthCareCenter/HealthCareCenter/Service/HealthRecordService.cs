using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Model;

namespace HealthCareCenter.Service
{
    public static class HealthRecordService
    {
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
