using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Service;

namespace HealthCareCenter.Model
{
    public class Room
    {
        public int ID { get; set; }
        public Dictionary<string, int> EquipmentAmounts { get; set; }
        public List<int> EquipmentRearrangementsIDs { get; set; }

        public Room()
        { }

        public bool IsStorage()
        {
            if (this.ID == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checking does room contains any equipment.
        /// </summary>
        /// <returns></returns>
        public bool ContainAnyEquipment()
        {
            if (EquipmentAmounts.Count != 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checking does room contains equipment. For example is room contains chair.
        /// </summary>
        /// <returns>True if contains or false if not</returns>
        public bool ContainsEquipment(string equipmentName)
        {
            if (!EquipmentAmounts.ContainsKey(equipmentName) || (EquipmentAmounts[equipmentName] == 0))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// For example, returns how much chairs are in room.
        /// </summary>
        /// <param name="equipmentName"></param>
        /// <returns></returns>
        public int GetEquipmentAmount(string equipmentName)
        {
            if (!this.ContainsEquipment(equipmentName))
            {
                return 0;
            }

            return EquipmentAmounts[equipmentName];
        }

        /// <summary>
        /// Check if room contain specific equipment. For example check if room contain equipment with id=1.
        /// </summary>
        /// <param name="specificEquipment"></param>
        /// <returns></returns>
        public bool Contains(Equipment specificEquipment)
        {
            List<Equipment> equipments = EquipmentService.GetEquipments();
            foreach (Equipment equipment in equipments)
            {
                if (equipment.CurrentRoomID == ID)
                {
                    if (equipment.ID == specificEquipment.ID)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Reduce equipment amount in dictionary
        /// </summary>
        /// <param name="equipment"></param>
        private void ReduceEquipmentAmount(Equipment equipment, Room room)
        {
            try
            {
                if (room.Contains(equipment))
                {
                    room.EquipmentAmounts[equipment.Name]--;
                    RoomService.Update(room);
                }
                else
                {
                    throw new EquipmentNotFound();
                }
            }
            catch (EquipmentNotFound ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void IncreaseEquipmentAmount(Equipment equipment, Room room)
        {
            if (room.ContainsEquipment(equipment.Name))
            {
                room.EquipmentAmounts[equipment.Name]++;
            }
            else
            {
                if (room.EquipmentAmounts.ContainsKey(equipment.Name) == false)
                {
                    room.EquipmentAmounts.Add(equipment.Name, 1);
                }
                else
                {
                    room.EquipmentAmounts[equipment.Name]++;
                }
            }
        }

        private void AddEquipment(Equipment equipment, Room room)
        {
            IncreaseEquipmentAmount(equipment, room);
            RoomService.Update(this);
            equipment.CurrentRoomID = room.ID;
            EquipmentService.Update(equipment);
        }

        private void TransferEquipment(Equipment equipment, Room room)
        {
            ReduceEquipmentAmount(equipment, this);
            AddEquipment(equipment, room);
        }

        public void TransferAllEquipment(Room room)
        {
            List<Equipment> equipments = EquipmentService.GetEquipments();
            for (int i = 0; i < equipments.Count; i++)
            {
                if (equipments[i].CurrentRoomID == this.ID)
                {
                    TransferEquipment(equipments[i], room);
                }
            }
        }

        public bool ContaninsAnyRearrangement()
        {
            List<EquipmentRearrangement> rearrangements = EquipmentRearrangementService.GetRearrangements();
            foreach (EquipmentRearrangement rearrangement in rearrangements)
            {
                if (rearrangement.OldRoomID == this.ID || rearrangement.NewRoomID == this.ID)
                {
                    return true;
                }
            }
            return false;
        }

        public List<Equipment> GetAllEquipments()
        {
            List<Equipment> roomEquipments = new List<Equipment>();
            List<Equipment> equipments = EquipmentService.GetEquipments();
            foreach (Equipment equipment in equipments)
            {
                if (equipment.CurrentRoomID == this.ID)
                {
                    roomEquipments.Add(equipment);
                }
            }

            return roomEquipments;
        }
    }
}