using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Enums;

namespace HealthCareCenter
{
    public class Equipment
    {
        public int ID { get; set; }
        public EquipmentType Type { get; set; }
        public string Name { get; set; }
        public int MoveInfoID { get; set; }
    }
}