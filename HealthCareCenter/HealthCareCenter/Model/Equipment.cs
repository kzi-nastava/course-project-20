using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Enums;
using HealthCareCenter.Service;

namespace HealthCareCenter.Model
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
            this.ID = EquipmentRepository.GetLargestEquipmentId() + 1;
            this.CurrentRoomID = 0;
            this.Type = type;
            this.Name = name;
            this.RearrangementID = -1;
        }

        /// <summary>
        /// Add all fields to list.
        /// </summary>
        /// <returns>Equipment object like list</returns>
        public List<string> ToList()
        {
            return new List<string> { this.ID.ToString(), this.CurrentRoomID.ToString(), this.Type.ToString(), this.Name };
        }
    }
}