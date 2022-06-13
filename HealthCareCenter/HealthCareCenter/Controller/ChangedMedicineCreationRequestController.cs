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
            SendRequest(request);

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

        public List<List<string>> GetRequestsForDisplay()
        {
            List<List<string>> requestsAttributesForDisplay = new List<List<string>>();
            foreach (MedicineCreationRequest request in MedicineCreationRequestRepository.Requests)
            {
                if (request.State == Enums.RequestState.Denied)
                {
                    List<string> attributes = new List<string>();
                    attributes.Add(request.ID.ToString());
                    attributes.Add(request.Name);
                    requestsAttributesForDisplay.Add(attributes);
                }
            }

            return requestsAttributesForDisplay;
        }

        private void SendRequest(MedicineCreationRequest request)
        {
            MedicineCreationRequestService.Delete(request);
            MedicineCreationRequestService.Add(request);
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