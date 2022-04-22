using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Enums;

namespace HealthCareCenter.Model
{
    public class DynamicEquipmentRequest
    {
        public int ID { get; set; }
        public RequestState State { get; set; }
        public int SecretaryID { get; set; }
        public Dictionary<int, int> EquipmentIDsAmounts { get; set; }
    }
}