using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Core.Equipment.Repositories;

namespace HealthCareCenter.Core.Equipment.Models
{
    public class Equipment
    {
        public int ID { get; set; }
        public int CurrentRoomID { get; set; }
        public EquipmentType Type { get; set; }
        public string Name { get; set; }
        public int RearrangementID { get; set; }

        public Equipment()
        { }

        /// <summary>
        /// Constructor used for adding new equipment.
        /// </summary>
        public Equipment(EquipmentType type, string name)
        {
            ID = EquipmentRepository.GetLargestEquipmentId() + 1;
            CurrentRoomID = 0;
            Type = type;
            Name = name;
            RearrangementID = -1;
        }

        /// <summary>
        /// Add all fields to list.
        /// </summary>
        /// <returns>Equipment object like list</returns>
        public List<string> ToList()
        {
            return new List<string> { ID.ToString(), CurrentRoomID.ToString(), Type.ToString(), Name };
        }
    }
}