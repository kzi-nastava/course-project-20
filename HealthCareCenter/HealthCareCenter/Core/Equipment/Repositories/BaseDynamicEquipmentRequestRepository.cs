using HealthCareCenter.Core.Equipment.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Equipment.Repositories
{
    public abstract class BaseDynamicEquipmentRequestRepository
    {
        public List<DynamicEquipmentRequest> Requests { get; set; }
        public int maxID = -1;
        public abstract void CalculateMaxID();
        public abstract void Load();
        public abstract void Save();
    }
}
