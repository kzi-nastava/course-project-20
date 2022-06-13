using HealthCareCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Exceptions;
using HealthCareCenter.Service;

namespace HealthCareCenter.Controller
{
    internal class EquipmentIrrevocableRearrangementContoller
    {
        public List<Equipment> SplitRoomEquipments { get; set; }
        public HospitalRoom SplitRoom { get; set; }
        public HospitalRoom Room1 { get; set; }
        public HospitalRoom Room2 { get; set; }
        public DateTime FinishDate { get; set; }

        public EquipmentIrrevocableRearrangementContoller(List<Equipment> splitRoomEquipments, HospitalRoom splitRoom, HospitalRoom room1, HospitalRoom room2, DateTime finishDate)
        {
            SplitRoomEquipments = splitRoomEquipments;
            SplitRoom = splitRoom;
            Room1 = room1;
            Room2 = room2;
            FinishDate = finishDate;
        }

        public void Transfer(string equipmentId, string newRoom)
        {
            if (IsSplitRoomContainsEquipment())
            {
                IsEquipmentValide(equipmentId);
                int parsedEquipmentId = Convert.ToInt32(equipmentId);
                Equipment equipment = EquipmentService.Get(parsedEquipmentId);
                DoIrrevocableRearrangement(newRoom, equipment);
            }
        }

        public bool IsSplitRoomContainsEquipment()
        {
            return SplitRoomEquipments.Count != 0;
        }

        private bool IsEqupmentIdInputValide(string equipmentId)
        {
            return Int32.TryParse(equipmentId, out int _);
        }

        private bool IsEquipmentFound(Equipment equipment)
        {
            foreach (Equipment splitRoomEquipment in SplitRoomEquipments)
            {
                if (splitRoomEquipment.ID == equipment.ID)
                {
                    return true;
                }
            }
            return false;
        }

        private void IsEquipmentValide(string equipmentId)
        {
            if (!IsEqupmentIdInputValide(equipmentId))
            {
                throw new InvalideEquipmentIdExcpetion(equipmentId);
            }
            int parsedEquipmentId = Convert.ToInt32(equipmentId);
            Equipment equipment = EquipmentService.Get(parsedEquipmentId);
            if (!IsEquipmentFound(equipment))
            {
                throw new EquipmentNotFoundException(equipmentId);
            }
        }

        private void SetIrrevocableRearrangement(Equipment equipment, int roomId)
        {
            SplitRoomEquipments.Remove(equipment);
            EquipmentRearrangement rearrangement = new EquipmentRearrangement(equipment, FinishDate, roomId);
            EquipmentRearrangementService.Set(rearrangement, equipment);
        }

        private void DoIrrevocableRearrangement(string newRoom, Equipment equipment)
        {
            if (newRoom == Room1.Name)
            {
                SetIrrevocableRearrangement(equipment, Room1.ID);
            }
            else if (newRoom == Room2.Name)
            {
                SetIrrevocableRearrangement(equipment, Room2.ID);
            }
        }
    }
}