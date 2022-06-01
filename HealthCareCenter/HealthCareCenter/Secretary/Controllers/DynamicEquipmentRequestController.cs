using HealthCareCenter.Model;
using HealthCareCenter.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Secretary.Controllers
{
    public class DynamicEquipmentRequestController
    {
        public DynamicEquipmentRequestController()
        {
            DynamicEquipmentRequestRepository.CalculateMaxID();
        }

        public void Send(List<string> request, Model.Secretary secretary)
        {
            if (request.Count == 0)
            {
                throw new Exception("You must first add equipment to the request.");
            }
            DynamicEquipmentRequestService.Send(request, secretary);
        }

        /// <summary>
        /// Tries to see if the selected equipment with the selected quantity could be added to the request.
        /// </summary>
        /// <param name="selectedEquipment">Selected equipment to add to request</param>
        /// <param name="selectedQuantity">Selected quantity of equipment to add to request</param>
        /// <param name="request">The current request</param>
        /// <returns>Quantity of the equipment to add to request, as an integer</returns>
        public int Add(string selectedEquipment, string selectedQuantity, List<string> request)
        {
            if (!int.TryParse(selectedQuantity, out int quantity) || quantity <= 0)
            {
                throw new Exception("Quantity must be a positive number.");
            }
            if (DynamicEquipmentRequestService.IsAlreadyAdded(selectedEquipment, request))
            {
                throw new Exception("You cannot add equipment that you have already added.");
            }
            return quantity;
        }

        public List<string> GetMissingEquipment()
        {
            return DynamicEquipmentRequestService.GetMissingEquipment();
        }
    }
}
