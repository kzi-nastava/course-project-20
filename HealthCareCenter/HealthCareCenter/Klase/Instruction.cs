using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Enums;

namespace HealthCareCenter
{
    public class Instruction
    {
        public string _comment { get; set; }
        public List<DateTime> _timeToTake { get; set; } // look again
        public int _dailyConsumption { get; set; }
        public ConsumptionPeriod _consumptionPeriod { get; set; }
    }
}