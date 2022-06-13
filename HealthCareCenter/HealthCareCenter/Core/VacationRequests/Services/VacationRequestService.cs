using HealthCareCenter.Core.Notifications.Services;
using HealthCareCenter.Core.Users;
using HealthCareCenter.Core.Users.Models;
using HealthCareCenter.Core.VacationRequests.Models;
using HealthCareCenter.Core.VacationRequests.Repositories;
using System;
using System.Collections.ObjectModel;

namespace HealthCareCenter.Core.VacationRequests.Services
{
    public class VacationRequestService : IVacationRequestService
    {
        INotificationService _notificationService;
        BaseVacationRequestRepository _vacationRequestRepository;

        public VacationRequestService(INotificationService notificationService, BaseVacationRequestRepository vacationRequestRepository)
        {
            _notificationService = notificationService;
            _vacationRequestRepository = vacationRequestRepository;
        }

        public bool OnVacation(int doctorID, DateTime when)
        {
            foreach (VacationRequest request in _vacationRequestRepository.Requests)
            {
                if (request.DoctorID == doctorID && request.State == RequestState.Approved
                    && request.StartDate.CompareTo(when) <= 0 && request.EndDate.CompareTo(when) >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        public ObservableCollection<VacationRequestDisplay> Get()
        {
            ObservableCollection<VacationRequestDisplay> vacationRequests = new ObservableCollection<VacationRequestDisplay>();

            foreach (VacationRequest request in _vacationRequestRepository.Requests)
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

        private void LinkDoctor(VacationRequest request, VacationRequestDisplay requestDisplay)
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

        public ObservableCollection<VacationRequestDisplay> Accept(int id)
        {
            PerformAccept(id);
            return Get();
        }

        private void PerformAccept(int id)
        {
            VacationRequest acceptedRequest = null;
            foreach (VacationRequest request in _vacationRequestRepository.Requests)
            {
                if (request.ID == id)
                {
                    request.State = RequestState.Approved;
                    acceptedRequest = request;
                    break;
                }
            }
            _vacationRequestRepository.Save();
            _notificationService.Send(acceptedRequest.DoctorID, $"The vacation you had requested is accepted, and starts on {acceptedRequest.StartDate.ToShortDateString()}, lasting until {acceptedRequest.EndDate.ToShortDateString()}.");
        }

        public ObservableCollection<VacationRequestDisplay> Deny(int id, string reason)
        {
            PerformDeny(id, reason);
            return Get();
        }

        private void PerformDeny(int id, string reason)
        {
            VacationRequest deniedRequest = null;
            foreach (VacationRequest request in _vacationRequestRepository.Requests)
            {
                if (request.ID == id)
                {
                    request.State = RequestState.Denied;
                    request.DenialReason = reason;
                    deniedRequest = request;
                    break;
                }
            }
            _vacationRequestRepository.Save();
            _notificationService.Send(deniedRequest.DoctorID, $"The vacation you had requested, which would have started on {deniedRequest.StartDate.ToShortDateString()} is denied. Reasoning: {deniedRequest.DenialReason}");
        }
    }
}
