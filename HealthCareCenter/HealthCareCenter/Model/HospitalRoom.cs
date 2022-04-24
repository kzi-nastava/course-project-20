using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Enums;

namespace HealthCareCenter.Model
{
    public class HospitalRoom : Room
    {
        public RoomType Type { get; set; }

        public string Name { get; set; }
        public List<int> EquipmentRearrangementsIDs { get; set; }
        public List<int> AppointmentIDs { get; set; }

        public HospitalRoom()
        { }

        /// <summary>
        /// Constructor used for adding new hospital room.
        /// </summary>
        /// <param name="type"></param>
        public HospitalRoom(RoomType type, string name)
        {
            this.ID = HospitalRoomRepository.GetLastRoomId() + 1;
            this.Name = name;
            this.EquipmentIDsAmounts = new Dictionary<int, int>();
            this.MoveInfoIDs = new List<int>();
            this.Type = type;
            this.EquipmentRearrangementsIDs = new List<int>();
            this.AppointmentIDs = new List<int>();
        }
    }
}