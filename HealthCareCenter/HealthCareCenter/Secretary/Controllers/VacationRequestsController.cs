using HealthCareCenter.Service;
using HealthCareCenter.Model;
using System;
using System.Collections.ObjectModel;

namespace HealthCareCenter.Secretary.Controllers
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
