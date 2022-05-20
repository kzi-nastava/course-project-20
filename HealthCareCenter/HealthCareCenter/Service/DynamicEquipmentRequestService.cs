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
    }
}
