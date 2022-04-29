using System;
using System.Collections.Generic;
using System.Text;

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
                return true;

            return false;
        }

        /// <summary>
        /// Checking does room contains any equipment.
        /// </summary>
        /// <returns></returns>
        public bool ContainAnyEquipment()
        {
            if (EquipmentAmounts.Count != 0)
                return true;

            return false;
        }

        /// <summary>
        /// Checking does room contains equipment. For example is room contains chair.
        /// </summary>
        /// <returns>True if contains or false if not</returns>
        public bool Contains(string equipmentName)
        {
            if (!EquipmentAmounts.ContainsKey(equipmentName) || (EquipmentAmounts[equipmentName] == 0))
                return false;
            return true;
        }

        /// <summary>
        /// For example, returns how much chairs are in room.
        /// </summary>
        /// <param name="equipmentName"></param>
        /// <returns></returns>
        public int GetEquipmentAmount(string equipmentName)
        {
            if (!this.Contains(equipmentName))
                return 0;

            return EquipmentAmounts[equipmentName];
        }

        /// <summary>
        /// Check if room contain specific equipment. For example check if room contain equipment with id=1.
        /// </summary>
        /// <param name="specificEquipment"></param>
        /// <returns></returns>
        public bool Contains(Equipment specificEquipment)
        {
            List<Equipment> equipments = EquipmentRepository.GetEquipments();
            foreach (Equipment equipment in equipments)
            {
                if (equipment.CurrentRoomID == ID)
                {
                    if (equipment.ID == specificEquipment.ID)
                        return true;
                }
            }
            return false;
        }
    }
}