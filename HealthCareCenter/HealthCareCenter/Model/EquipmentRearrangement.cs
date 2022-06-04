using HealthCareCenter.Service;
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

        public EquipmentRearrangement()
        { }

        public EquipmentRearrangement(Equipment equipment, DateTime moveTime, int newRoomID)
        {
            this.ID = EquipmentRearrangementRepository.GetLargestID() + 1;
            this.EquipmentID = equipment.ID;
            this.MoveTime = moveTime;
            this.OldRoomID = equipment.CurrentRoomID;
            this.NewRoomID = newRoomID;
        }

        public EquipmentRearrangement(int equipmentID, DateTime moveTime, int oldRoomID, int newRoomID)
        {
            this.ID = EquipmentRearrangementRepository.GetLargestID() + 1;
            this.EquipmentID = equipmentID;
            this.MoveTime = moveTime;
            this.OldRoomID = oldRoomID;
            this.NewRoomID = newRoomID;
        }
    }
}