using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Enums;

namespace HealthCareCenter.Model
{
    public class DynamicEquipmentRequest
    {
        public int ID { get; set; }
        public bool Fulfilled { get; set; }
        public int SecretaryID { get; set; }
        public DateTime Created { get; set; }
        public Dictionary<string, int> AmountOfEquipment { get; set; }
    }
}