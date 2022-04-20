using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Enums;

namespace HealthCareCenter
{
    internal class Equipment
    {
        public EquipmentType equipmentType;
        public string equipmentName;
        public List<EquipmentMoveTime> moves; // gde da ga mrda
    }
}