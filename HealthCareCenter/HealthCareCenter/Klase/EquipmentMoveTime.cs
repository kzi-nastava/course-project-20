using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter
{
    public class EquipmentMoveTime // change class name;
    {
        public DateTime _moveTime { get; set; } // look again => time
        public Equipment _equipment { get; set; }
        public HospitalRoom _room { get; set; }
        public Storage _storage { get; set; }
    }
}