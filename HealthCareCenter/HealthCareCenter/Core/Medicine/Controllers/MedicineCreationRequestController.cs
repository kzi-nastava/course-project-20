using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Core.Medicine.Models;
using HealthCareCenter.Core.Medicine.Services;

namespace HealthCareCenter.Core.Medicine.Controllers
{
    public class MedicineCreationRequestController : BaseMedicineCreationRequestController
    {
        private IMedicineCreationRequestService _medicineCreationRequestService;

        public MedicineCreationRequestController(IMedicineCreationRequestService medicineCreationRequestService)
        {
            _medicineCreationRequestService = medicineCreationRequestService;
            AddedIngrediens = new List<string>();
        }

        public override void Send(string medicineName, string medicineManufacturer)
        {
            IsPossibleToCreateMedicineCreationRequest(medicineName, medicineManufacturer);
            MedicineCreationRequest medicineCreationRequest = new MedicineCreationRequest(
                medicineName, AddedIngrediens, medicineManufacturer, RequestState.Waiting);
            _medicineCreationRequestService.Add(medicineCreationRequest);
            AddedIngrediens.Clear();
        }
    }
}