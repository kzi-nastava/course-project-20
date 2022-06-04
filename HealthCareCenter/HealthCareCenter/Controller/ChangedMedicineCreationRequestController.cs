using HealthCareCenter.Model;
using HealthCareCenter.Service;
using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Exceptions;

namespace HealthCareCenter.Controller
{
    public class ChangedMedicineCreationRequestController : BaseMedicineCreationRequestController
    {
        public MedicineCreationRequest DisplayedRequest { get; set; }

        public override void Send(string medicineName, string manufacturer)
        {
            IsPossibleToCreateMedicineCreationRequest(medicineName, manufacturer);
            MedicineCreationRequest request = new MedicineCreationRequest(
              DisplayedRequest.ID, medicineName,
              DisplayedRequest.Ingredients, manufacturer,
              Enums.RequestState.Waiting);

            MedicineCreationRequestService.Delete(request);
            MedicineCreationRequestService.Add(request);

            AddedIngrediens.Clear();
            DisplayedRequest = null;
        }

        public void DisplayRequest(string requestId)
        {
            IsRequestValide(requestId);
            int parsedChangeRequestId = Convert.ToInt32(requestId);
            DisplayedRequest = MedicineCreationRequestService.Get(parsedChangeRequestId);
            AddedIngrediens = DisplayedRequest.Ingredients;
        }

        private bool IsRequestIdValide(string id)
        {
            return Int32.TryParse(id, out _);
        }

        private bool IsRequestFound(string id)
        {
            int parsedId = Convert.ToInt32(id);
            MedicineCreationRequest request = MedicineCreationRequestService.Get(parsedId);

            if (request == null) { return false; }
            else if (request.State != Enums.RequestState.Denied) { return false; }

            return true;
        }

        private void IsRequestValide(string id)
        {
            if (!IsRequestIdValide(id)) { throw new MedicineCreationRequestNotFoundException(id); }

            if (!IsRequestFound(id)) { throw new MedicineCreationRequestNotFoundException(id); }
        }
    }
}