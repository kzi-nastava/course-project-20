using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Enums;

namespace HealthCareCenter.Model
{
    public class MedicineInstruction
    {
        public MedicineInstruction() { }
        public MedicineInstruction(int _id, string _comment, List<DateTime> _consumptionTime, int _dailyConsumption, ConsumptionPeriod _consumptionPeriod)
        {
            this.ID = _id;
            this.Comment = _comment;
            this.ConsumptionTime = new List<DateTime>(_consumptionTime);
            this.DailyConsumption = _dailyConsumption;
            this.ConsumptionPeriod = _consumptionPeriod;
        }
        public int ID { get; set; }
        public int MedicineID { get; set; }
        public string Comment { get; set; }
        public List<DateTime> ConsumptionTime { get; set; }
        public int DailyConsumption { get; set; }
        public ConsumptionPeriod ConsumptionPeriod { get; set; }
    }
}