using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Enums;

namespace HealthCareCenter
{
    public class MedicineInstruction
    {
        public int ID { get; set; }
        public string Comment { get; set; }
        public List<DateTime> ConsumptionTime { get; set; }
        public int DailyConsumption { get; set; }
        public ConsumptionPeriod ConsumptionPeriod { get; set; }
    }
}