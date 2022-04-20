using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Enums;

namespace HealthCareCenter
{
    public class Equipment
    {
        public int id { get; set; }
        public EquipmentType _type { get; set; }
        public string _name { get; set; }
        public List<EquipmentMoveTime> movesInfo { get; set; } // look again
    }
}