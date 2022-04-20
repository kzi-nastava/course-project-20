using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Enums;

namespace HealthCareCenter
{
    internal class HospitalRooom
    {
        public int _roomId;
        public RoomType _roomType;
        public List<Equipment> allEquipment;
        public List<EquipmentMoveTime> equipmentToMove;
        public List<Appointment> appoinmentScheduled;
    }
}