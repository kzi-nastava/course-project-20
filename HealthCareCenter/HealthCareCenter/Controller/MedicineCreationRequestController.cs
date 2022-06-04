using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Exceptions;
using HealthCareCenter.Model;
using HealthCareCenter.Service;

namespace HealthCareCenter.Controller
{
    public class MedicineCreationRequestController : BaseMedicineCreationRequestController
    {
        public MedicineCreationRequestController()
        {
            AddedIngrediens = new List<string>();
        }

        public override void Send(string medicineName, string medicineManufacturer)
        {
            IsPossibleToCreateMedicineCreationRequest(medicineName, medicineManufacturer);
            MedicineCreationRequest medicineCreationRequest = new MedicineCreationRequest(
                medicineName, AddedIngrediens, medicineManufacturer, Enums.RequestState.Waiting);
            MedicineCreationRequestService.Add(medicineCreationRequest);
            AddedIngrediens.Clear();
        }
    }
}