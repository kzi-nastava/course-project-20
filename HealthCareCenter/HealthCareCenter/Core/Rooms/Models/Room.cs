using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Rooms.Models
{
    public class Room
    {
        public int ID { get; set; }
        public Dictionary<string, int> EquipmentAmounts { get; set; }
        public List<int> EquipmentRearrangementsIDs { get; set; }

        public Room()
        { }
    }
}