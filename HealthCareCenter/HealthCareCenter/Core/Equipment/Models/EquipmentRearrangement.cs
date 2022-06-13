using HealthCareCenter.Core.Equipment.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Equipment.Models
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
            ID = EquipmentRearrangementRepository.GetLargestID() + 1;
            EquipmentID = equipment.ID;
            MoveTime = moveTime;
            OldRoomID = equipment.CurrentRoomID;
            NewRoomID = newRoomID;
        }

        public EquipmentRearrangement(int equipmentID, DateTime moveTime, int oldRoomID, int newRoomID)
        {
            ID = EquipmentRearrangementRepository.GetLargestID() + 1;
            EquipmentID = equipmentID;
            MoveTime = moveTime;
            OldRoomID = oldRoomID;
            NewRoomID = newRoomID;
        }
    }
}