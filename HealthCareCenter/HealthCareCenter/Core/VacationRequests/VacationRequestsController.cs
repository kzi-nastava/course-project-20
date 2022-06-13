using System;
using System.Collections.ObjectModel;
using HealthCareCenter.Core.VacationRequests.Models;
using HealthCareCenter.Core.VacationRequests.Services;

namespace HealthCareCenter.Core.VacationRequests
{
    public class VacationRequestsController
    {
        private IVacationRequestService _vacationRequestService;

        public VacationRequestsController(IVacationRequestService vacationRequestService)
        {
            _vacationRequestService = vacationRequestService;
        }

        public ObservableCollection<VacationRequestDisplay> Get()
        {
            return _vacationRequestService.Get();
        }

        public ObservableCollection<VacationRequestDisplay> Accept(int id)
        {
            return _vacationRequestService.Accept(id);
        }

        public ObservableCollection<VacationRequestDisplay> Deny(int id, string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
            {
                throw new Exception("You must enter the reason for denial first.");
            }
            return _vacationRequestService.Deny(id, reason);
        }
    }
}
