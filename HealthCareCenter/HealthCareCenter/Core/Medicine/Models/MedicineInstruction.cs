using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Medicine.Models
{
    public class MedicineInstruction
    {
        public MedicineInstruction() { }
        public MedicineInstruction(int _id, string _comment, List<DateTime> _consumptionTime, int _dailyConsumption, ConsumptionPeriod _consumptionPeriod, int _medicineID)
        {
            ID = _id;
            Comment = _comment;
            MedicineID = _medicineID;
            ConsumptionTime = new List<DateTime>(_consumptionTime);
            DailyConsumption = _dailyConsumption;
            ConsumptionPeriod = _consumptionPeriod;
        }
        public int ID { get; set; }
        public int MedicineID { get; set; }
        public string Comment { get; set; }
        public List<DateTime> ConsumptionTime { get; set; }
        public int DailyConsumption { get; set; }
        public ConsumptionPeriod ConsumptionPeriod { get; set; }
    }
}