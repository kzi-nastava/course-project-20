using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Model;

namespace HealthCareCenter.Service
{
    internal class MedicineCreationRequestService
    {
        public static MedicineCreationRequest getMedicineCreationRequest(int id)
        {
            foreach(MedicineCreationRequest request in MedicineCreationRequestRepository.Requests)
            {
                if (id == request.ID)
                    return request;
            }
            return null;
        }
    }
}
