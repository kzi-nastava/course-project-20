using HealthCareCenter.Core.Rooms.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Equipment.Services
{
    public interface IDynamicEquipmentService
    {
        void FulfillRequestsIfNeeded();
        bool IsAlreadyAdded(string selectedEquipment, List<string> request);
        void SendRequest(List<string> request, Users.Models.Secretary secretary);
        List<string> GetMissingEquipment();
        void Transfer(int quantity, string equipment, Room transferFrom, Room transferTo, Room storage);
    }
}
