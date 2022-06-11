using HealthCareCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Service
{
    public interface IDynamicEquipmentService
    {
        void FulfillRequestsIfNeeded();
        bool IsAlreadyAdded(string selectedEquipment, List<string> request);
        void SendRequest(List<string> request, Model.Secretary secretary);
        List<string> GetMissingEquipment();
        void Transfer(int quantity, string equipment, Room transferFrom, Room transferTo, Room storage);
    }
}
