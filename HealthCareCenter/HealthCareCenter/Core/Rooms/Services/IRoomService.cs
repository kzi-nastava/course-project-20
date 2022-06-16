using HealthCareCenter.Core.Equipment.Services;
using HealthCareCenter.Core.Rooms.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Rooms
{
    public interface IRoomService
    {
        Dictionary<string, int> GetEquipmentsAmount();

        bool Update(Room room);

        Room Get(int roomId);

        Room GetPremesisForEquipmentTransfer(int roomId);

        bool IsStorage(Room room);

        bool ContainAnyEquipment(Room room);

        bool ContainsEquipment(Room room, string equipmentName);

        int GetEquipmentAmount(Room room, string equipmentName);

        bool Contains(Room room, Equipment.Models.Equipment specificEquipment);

        void TransferAllEquipment(Room currentEquipmentRoom, Room newEquipmentRoom);

        bool ContainsAnyRearrangement(Room room, IEquipmentRearrangementService equipmentRearrangementService);

        List<Equipment.Models.Equipment> GetAllEquipment(Room room);
    }
}