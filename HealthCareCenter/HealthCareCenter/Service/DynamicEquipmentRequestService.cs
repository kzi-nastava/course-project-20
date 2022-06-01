using HealthCareCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Service
{
    public class DynamicEquipmentRequestService
    {
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

        public static bool IsAlreadyAdded(string selectedEquipment, List<string> request)
        {
            foreach (string equipmentWithQuantity in request)
            {
                string equipment = equipmentWithQuantity.Split(":")[0];
                if (selectedEquipment == equipment)
                {
                    return true;
                }
            }
            return false;
        }

        public static void Send(List<string> request, Model.Secretary secretary)
        {
            Dictionary<string, int> amountOfEquipment = GetAmountOfEquipment(request);
            Send(amountOfEquipment, secretary);
        }

        private static void Send(Dictionary<string, int> amountOfEquipment, Model.Secretary secretary)
        {
            DynamicEquipmentRequest request = new DynamicEquipmentRequest(++DynamicEquipmentRequestRepository.maxID, false, secretary.ID, DateTime.Now, amountOfEquipment);
            DynamicEquipmentRequestRepository.Requests.Add(request);
            DynamicEquipmentRequestRepository.Save();
        }

        private static Dictionary<string, int> GetAmountOfEquipment(List<string> request)
        {
            Dictionary<string, int> amountOfEquipment = new Dictionary<string, int>();

            foreach (string equipmentWithQuantity in request)
            {
                var equipmentAndQuantity = equipmentWithQuantity.Split(":");
                string equipment = equipmentAndQuantity[0];
                int quantity = int.Parse(equipmentAndQuantity[1]);
                amountOfEquipment.Add(equipment, quantity);
            }

            return amountOfEquipment;
        }

        public static List<string> GetMissingEquipment()
        {
            Room storage = StorageRepository.Load();
            List<string> missingEquipment = new List<string>(Constants.DynamicEquipment);
            foreach (string equipment in storage.EquipmentAmounts.Keys)
            {
                if (storage.EquipmentAmounts[equipment] > 0)
                {
                    missingEquipment.Remove(equipment);
                }
            }
            return missingEquipment;
        }
    }
}
