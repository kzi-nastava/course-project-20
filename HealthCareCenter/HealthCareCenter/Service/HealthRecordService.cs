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
            foreach (HealthRecord record in HealthRecordRepository.Records)
            {
                if (appointment.HealthRecordID == record.ID)
                {
                    return record;
                }
            }
            return null;
        }
    }
}
