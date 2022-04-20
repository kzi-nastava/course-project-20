using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter
{
    class Instruction
    {
        public string comment { get; set; }
        public List<Date> timeToTake { get; set; }
        public int dailyConsumption { get; set; }
        public ConsumptionPeriod consumptionPeriod { get; set; }
    }
}
