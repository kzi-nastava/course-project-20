using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter
{
    public class Storage
    {
        public List<Equipment> _equipments { get; set; }
        public List<EquipmentMoveTime> _movesInfo { get; set; } // look again
    }
}