using HealthCareCenter.Model;
using HealthCareCenter.Service;
using System.Collections.Generic;

namespace HealthCareCenter.Secretary.Controllers
{
    public class ViewChangeRequestsController
    {
        private IAppointmentChangeRequestService _service;

        public ViewChangeRequestsController(IAppointmentChangeRequestService service)
        {
            _service = service;
            AppointmentChangeRequestRepository.Load();
            AppointmentRepository.Load();
        }

        public void Refresh(List<DeleteRequest> deleteRequests, List<EditRequest> editRequests, Patient patient)
        {
            _service.Refresh(deleteRequests, editRequests, patient);
        }

        public void RejectEditRequest(int requestID)
        {
            _service.RejectEditRequest(requestID);
        }

        public void AcceptEditRequest(int requestID)
        {
            _service.AcceptEditRequest(requestID);
        }

        public void RejectDeleteRequest(int requestID)
        {
            _service.RejectDeleteRequest(requestID);
        }

        public void AcceptDeleteRequest(int requestID)
        {
            _service.AcceptDeleteRequest(requestID);
        }
    }
}
