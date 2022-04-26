using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Model
{
    public class EquipmentRearrangement
    {
        public int ID { get; set; }
        public int EquipmentID { get; set; }
        public DateTime MoveTime { get; set; }
        public int OldRoomID { get; set; }
        public int NewRoomID { get; set; }
    }
}