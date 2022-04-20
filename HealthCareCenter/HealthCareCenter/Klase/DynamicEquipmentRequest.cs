using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Enums;

namespace HealthCareCenter
{
    public class DynamicEquipmentRequest
    {
        public RequestState _state { get; set; }
        public Secretary _secretary { get; set; }
        public List<Equipment> _equipment { get; set; } // look again => equipments
        public List<int> _amount { get; set; }
    }
}