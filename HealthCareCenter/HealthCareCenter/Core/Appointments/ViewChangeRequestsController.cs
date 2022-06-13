using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Appointments.Repository;
using HealthCareCenter.Core.Appointments.Services;
using HealthCareCenter.Core.Patients;
using System.Collections.Generic;

namespace HealthCareCenter.Core.Appointments
{
    public class ViewChangeRequestsController
    {
        private readonly IAppointmentChangeRequestService _service;

        public ViewChangeRequestsController(IAppointmentChangeRequestService service)
        {
            _service = service;
            AppointmentChangeRequestRepository.Load();
            AppointmentRepository.Load();
        }

        public void Refresh(List<DeleteRequestForDisplay> deleteRequests, List<EditRequestForDisplay> editRequests, Patient patient)
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
