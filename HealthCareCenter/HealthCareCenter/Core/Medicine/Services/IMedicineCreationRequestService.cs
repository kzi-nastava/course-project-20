using HealthCareCenter.Core.Medicine.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Medicine.Services
{
    public interface IMedicineCreationRequestService
    {
        MedicineCreationRequest GetMedicineCreationRequest(int id);

        string GetIngredients(MedicineCreationRequest request);

        MedicineCreationRequest Get(int id);

        List<MedicineCreationRequest> GetMedicines();

        void Add(MedicineCreationRequest newMedicineCreationRequest);

        bool Delete(int id);

        bool Delete(MedicineCreationRequest medicineCreationRequest);
    }
}