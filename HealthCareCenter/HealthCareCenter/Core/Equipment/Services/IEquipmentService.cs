using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Equipment.Services
{
    public interface IEquipmentService
    {
        Models.Equipment Get(int id);
        List<Models.Equipment> GetEquipments();
        void Add(Models.Equipment newEquipment);
        bool Delete(int id);
        bool Delete(Models.Equipment equipment);
        bool Update(Models.Equipment equipment);
        bool HasScheduledRearrangement(Models.Equipment equipment);
    }
}
