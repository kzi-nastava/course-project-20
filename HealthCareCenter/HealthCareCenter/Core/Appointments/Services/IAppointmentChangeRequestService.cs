using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Patients;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Appointments.Services
{
    public interface IAppointmentChangeRequestService
    {
        void DeleteAppointment(AppointmentChangeRequest request);
        void EditAppointment(AppointmentChangeRequest request);
        void Refresh(List<DeleteRequestForDisplay> deleteRequests, List<EditRequestForDisplay> editRequests, Patient patient);
        void RejectEditRequest(int requestID);
        void AcceptEditRequest(int requestID);
        void RejectDeleteRequest(int requestID);
        void AcceptDeleteRequest(int requestID);
    }
}
