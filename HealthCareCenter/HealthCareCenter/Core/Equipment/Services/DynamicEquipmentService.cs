using HealthCareCenter.Core.Equipment.Models;
using HealthCareCenter.Core.Equipment.Repositories;
using HealthCareCenter.Core.Rooms.Models;
using HealthCareCenter.Core.Rooms.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Equipment.Services
{
    public class DynamicEquipmentService : IDynamicEquipmentService
    {
        private BaseDynamicEquipmentRequestRepository _requestRepository;

        public DynamicEquipmentService(BaseDynamicEquipmentRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        public void FulfillRequestsIfNeeded()
        {
            Room storage = StorageRepository.Load();
            foreach (DynamicEquipmentRequest request in _requestRepository.Requests)
            {
                if (request.Fulfilled || request.Created.AddDays(1).CompareTo(DateTime.Now) > 0)
                {
                    continue;
                }
                FulfillRequest(storage, request);
            }
            _requestRepository.Save();
            StorageRepository.Save(storage);
        }

        private void FulfillRequest(Room storage, DynamicEquipmentRequest request)
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

        public bool IsAlreadyAdded(string selectedEquipment, List<string> request)
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

        public void SendRequest(List<string> request, Users.Models.Secretary secretary)
        {
            Dictionary<string, int> amountOfEquipment = GetAmountOfEquipment(request);
            SendRequest(amountOfEquipment, secretary);
        }

        private void SendRequest(Dictionary<string, int> amountOfEquipment, Users.Models.Secretary secretary)
        {
            DynamicEquipmentRequest request = new DynamicEquipmentRequest(++_requestRepository.maxID, false, secretary.ID, DateTime.Now, amountOfEquipment);
            _requestRepository.Requests.Add(request);
            _requestRepository.Save();
        }

        private Dictionary<string, int> GetAmountOfEquipment(List<string> request)
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

        public List<string> GetMissingEquipment()
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

        public void Transfer(int quantity, string equipment, Room transferFrom, Room transferTo, Room storage)
        {
            transferFrom.EquipmentAmounts[equipment] -= quantity;

            if (transferTo.EquipmentAmounts.ContainsKey(equipment))
                transferTo.EquipmentAmounts[equipment] += quantity;
            else
                transferTo.EquipmentAmounts.Add(equipment, quantity);

            StorageRepository.Save(storage);
            HospitalRoomRepository.Save();
        }
    }
}
