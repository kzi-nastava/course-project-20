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
    }
}