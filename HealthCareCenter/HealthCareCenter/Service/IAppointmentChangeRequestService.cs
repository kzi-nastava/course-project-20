using HealthCareCenter.Model;
using HealthCareCenter.Secretary;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Service
{
    public interface IAppointmentChangeRequestService
    {
        void Refresh(List<DeleteRequest> deleteRequests, List<EditRequest> editRequests, Patient patient);
        void RejectEditRequest(int requestID);
        void AcceptEditRequest(int requestID);
        void RejectDeleteRequest(int requestID);
        void AcceptDeleteRequest(int requestID);
    }
}
