using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Model
{
    public class Room
    {
        public int ID { get; set; }
        public Dictionary<int, int> EquipmentIDsAmounts { get; set; }
        public List<int> MoveInfoIDs { get; set; }
    }
}
