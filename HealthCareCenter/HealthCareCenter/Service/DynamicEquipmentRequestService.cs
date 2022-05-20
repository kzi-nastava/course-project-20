using HealthCareCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Service
{
    public class DynamicEquipmentRequestService
    {
        public static int maxID = -1;

        public static void CalculateMaxID()
        {
            maxID = -1;
            foreach (DynamicEquipmentRequest request in DynamicEquipmentRequestRepository.Requests)
            {
                if (request.ID > maxID)
                {
                    maxID = request.ID;
                }
            }
        }

        public static void FulfillRequestsIfNeeded()
        {
            Room storage = StorageRepository.Load();
            foreach (DynamicEquipmentRequest request in DynamicEquipmentRequestRepository.Requests)
            {
                if (request.Fulfilled || request.Created.AddDays(1).CompareTo(DateTime.Now) > 0)
                {
                    continue;
                }
                FulfillRequest(storage, request);
            }
            DynamicEquipmentRequestRepository.Save();
            StorageRepository.Save(storage);
        }

        private static void FulfillRequest(Room storage, DynamicEquipmentRequest request)
        {
            foreach (string equipment in request.AmountOfEquipment.Keys)
            {
                if (storage.EquipmentAmounts.ContainsKey(equipment))
                {
                    storage.EquipmentAmounts[equipment] += request.AmountOfEquipment[equipment];
                }
                else
                {
                    storage.EquipmentAmounts.Add(equipment, request.AmountOfEquipment[equipment]);
                }
            }
            request.Fulfilled = true;
        }
    }
}
