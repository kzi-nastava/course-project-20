using System;
using System.Collections.Generic;

namespace HealthCareCenter.Core.Equipment.Models
{
    public class DynamicEquipmentRequest
    {
        public int ID { get; set; }
        public bool Fulfilled { get; set; }
        public int SecretaryID { get; set; }
        public DateTime Created { get; set; }
        public Dictionary<string, int> AmountOfEquipment { get; set; }

        public DynamicEquipmentRequest() { }

        public DynamicEquipmentRequest(int id, bool fulfilled, int secretaryID, DateTime created, Dictionary<string, int> amountOfEquipment)
        {
            ID = id;
            Fulfilled = fulfilled;
            SecretaryID = secretaryID;
            Created = created;
            AmountOfEquipment = amountOfEquipment;
        }
    }
}