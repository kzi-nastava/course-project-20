using HealthCareCenter.Enums;
using HealthCareCenter.Model;
using HealthCareCenter.Secretary;
using HealthCareCenter.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace HealthCareCenter.Controller
{
    public static class VacationRequestService
    {
        public static bool OnVacation(int doctorID, DateTime when)
        {
            foreach (VacationRequest request in VacationRequestRepository.Requests)
            {
                if (request.DoctorID == doctorID && request.State == RequestState.Approved 
                    && request.StartDate.CompareTo(when) <= 0 && request.EndDate.CompareTo(when) >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static ObservableCollection<VacationRequestDisplay> Refresh()
        {
            ObservableCollection<VacationRequestDisplay> vacationRequests = new ObservableCollection<VacationRequestDisplay>();

            foreach (VacationRequest request in VacationRequestRepository.Requests)
            {
                if (request.State != RequestState.Waiting || request.StartDate.CompareTo(DateTime.Now) <= 0)
                    continue;
                VacationRequestDisplay requestDisplay = new VacationRequestDisplay()
                {
                    ID = request.ID,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    RequestReason = request.RequestReason
                };
                LinkDoctor(request, requestDisplay);
                vacationRequests.Add(requestDisplay);
            }

            return vacationRequests;
        }

        private static void LinkDoctor(VacationRequest request, VacationRequestDisplay requestDisplay)
        {
            foreach (Doctor doctor in UserRepository.Doctors)
            {
                if (doctor.ID == request.DoctorID)
                {
                    requestDisplay.DoctorName = doctor.FirstName + " " + doctor.LastName;
                    return;
                }
            }
        }

        public static ObservableCollection<VacationRequestDisplay> Accept(int id)
        {
            PerformAccept(id);
            return Refresh();
        }

        private static void PerformAccept(int id)
        {
            VacationRequest acceptedRequest = null;
            foreach (VacationRequest request in VacationRequestRepository.Requests)
            {
                if (request.ID == id)
                {
                    request.State = RequestState.Approved;
                    acceptedRequest = request;
                    break;
                }
            }
            VacationRequestRepository.Save();
            NotificationService.SendNotification(acceptedRequest.DoctorID, $"The vacation you had requested is accepted, and starts on {acceptedRequest.StartDate.ToShortDateString()}, lasting until {acceptedRequest.EndDate.ToShortDateString()}.");
        }

        public static ObservableCollection<VacationRequestDisplay> Deny(int id, string reason)
        {
            PerformDeny(id, reason);
            return Refresh();
        }

        private static void PerformDeny(int id, string reason)
        {
            VacationRequest deniedRequest = null;
            foreach (VacationRequest request in VacationRequestRepository.Requests)
            {
                if (request.ID == id)
                {
                    request.State = RequestState.Denied;
                    request.DenialReason = reason;
                    deniedRequest = request;
                    break;
                }
            }
            VacationRequestRepository.Save();
            NotificationService.SendNotification(deniedRequest.DoctorID, $"The vacation you had requested, which would have started on {deniedRequest.StartDate.ToShortDateString()} is denied. Reasoning: {deniedRequest.DenialReason}");
        }
    }
}
