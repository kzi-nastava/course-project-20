using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Enums;

namespace HealthCareCenter.Model
{
    public class HospitalRoom : Room
    {
        public RoomType Type { get; set; }
        public List<int> EquipmentRearrangementsIDs { get; set; }
        public List<int> AppointmentIDs { get; set; }
    }
}