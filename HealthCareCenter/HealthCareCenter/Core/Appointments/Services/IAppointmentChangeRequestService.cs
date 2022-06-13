using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Patients.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Appointments.Services
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
