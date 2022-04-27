using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Model;

namespace HealthCareCenter.Service
{
    public static class HealthRecordService
    {
        public static int maxHealthRecordID = -1;

        public static void CalculateMaxHealthRecordID()
        {
            maxHealthRecordID = -1;
            foreach (HealthRecord healthRecord in HealthRecordRepository.HealthRecords)
            {
                if (healthRecord.ID > maxHealthRecordID)
                {
                    maxHealthRecordID = healthRecord.ID;
                }
            }
        }
    }
}
