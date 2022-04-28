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

        public List<int> AppointmentIDs { get; set; }

        public HospitalRoom()
        { }

        /// <summary>
        /// Constructor used for adding new hospital room.
        /// </summary>
        public HospitalRoom(RoomType type, string name)
        {
            this.ID = HospitalRoomRepository.GetLastRoomId() + 1;
            this.Name = name;
            this.EquipmentAmounts = new Dictionary<string, int>();
            this.EquipmentRearrangementsIDs = new List<int>();
            this.Type = type;
            this.AppointmentIDs = new List<int>();
        }

        /// <summary>
        /// Check if room conatain any appointment
        /// </summary>
        /// <returns></returns>
        public bool ContainAnyAppointment()
        {
            if (AppointmentIDs.Count != 0)
                return true;
            return false;
        }
    }
}