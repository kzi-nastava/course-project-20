using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Enums;

namespace HealthCareCenter
{
    public class HospitalRoom
    {
        public int _id;
        public RoomType _type { get; set; }
        public List<Equipment> _equipments { get; set; }
        public List<EquipmentMoveTime> _equipmentToMove { get; set; } // look again
        public List<Appointment> _appoinmentScheduled { get; set; } // look again
    }
}