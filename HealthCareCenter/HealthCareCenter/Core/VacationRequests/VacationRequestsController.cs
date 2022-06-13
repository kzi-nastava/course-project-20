using System;
using System.Collections.ObjectModel;
using HealthCareCenter.Core.VacationRequests.Models;
using HealthCareCenter.Core.VacationRequests.Services;

namespace HealthCareCenter.Core.VacationRequests
{
    public class VacationRequestsController
    {
        private readonly IVacationRequestService _vacationRequestService;

        public VacationRequestsController(IVacationRequestService vacationRequestService)
        {
            _vacationRequestService = vacationRequestService;
        }

        public ObservableCollection<VacationRequestForDisplay> Get()
        {
            return _vacationRequestService.Get();
        }

        public ObservableCollection<VacationRequestForDisplay> Accept(int id)
        {
            return _vacationRequestService.Accept(id);
        }

        public ObservableCollection<VacationRequestForDisplay> Deny(int id, string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
            {
                throw new Exception("You must enter the reason for denial first.");
            }
            return _vacationRequestService.Deny(id, reason);
        }
    }
}
