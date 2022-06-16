using HealthCareCenter.Core.Equipment.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Equipment.Services
{
    public interface IEquipmentRearrangementService
    {
        List<EquipmentRearrangement> GetRearrangements();

        void Add(EquipmentRearrangement newRearrangement);

        EquipmentRearrangement Get(int id);

        bool Delete(int id);

        bool Update(EquipmentRearrangement rearrangement);

        void Remove(Models.Equipment equipment);

        void Set(EquipmentRearrangement rearrangement, Models.Equipment equipment);

        void DoPossibleRearrangement(Models.Equipment equipment);

        bool IsIrrevocable(EquipmentRearrangement rearrangement);
    }
}