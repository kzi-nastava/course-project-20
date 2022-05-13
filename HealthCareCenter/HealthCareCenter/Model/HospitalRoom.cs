using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Enums;
using HealthCareCenter.Service;
using System.Linq;

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
            List<int> largestIDs = new List<int> { HospitalRoomService.GetLargestRoomId(), HospitalRoomForRenovationService.GetLargestRoomId(), HospitalRoomUnderConstructionService.GetLargestRoomId() };

            this.ID = largestIDs.Max() + 1;
            this.Name = name;
            this.EquipmentAmounts = new Dictionary<string, int>();
            this.EquipmentRearrangementsIDs = new List<int>();
            this.Type = type;
            this.AppointmentIDs = new List<int>();
        }

        /// <summary>
        /// Check if room contains any appointment
        /// </summary>
        /// <returns></returns>
        public bool ContainsAnyAppointment()
        {
            if (AppointmentIDs.Count != 0)
            {
                return true;
            }

            return false;
        }

        public bool IsCurrentlyRenovating()
        {
            foreach (HospitalRoom room in HospitalRoomForRenovationService.GetRooms())
            {
                if (room.ID == this.ID)
                {
                    return true;
                }
            }

            return false;
        }
    }
}