using HealthCareCenter.Model;
using System.Collections.Generic;

namespace HealthCareCenter.Secretary.Controllers
{
    public class ViewChangeRequestsController
    {
        public ViewChangeRequestsController()
        {
            AppointmentChangeRequestRepository.Load();
            AppointmentRepository.Load();
        }

        public void Refresh(List<DeleteRequest> deleteRequests, List<EditRequest> editRequests, Patient patient)
        {
            AppointmentChangeRequestService.Refresh(deleteRequests, editRequests, patient);
        }

        public void RejectEditRequest(int requestID)
        {
            AppointmentChangeRequestService.RejectEditRequest(requestID);
        }

        public void AcceptEditRequest(int requestID)
        {
            AppointmentChangeRequestService.AcceptEditRequest(requestID);
        }

        public void RejectDeleteRequest(int requestID)
        {
            AppointmentChangeRequestService.RejectDeleteRequest(requestID);
        }

        public void AcceptDeleteRequest(int requestID)
        {
            AppointmentChangeRequestService.AcceptDeleteRequest(requestID);
        }
    }
}
