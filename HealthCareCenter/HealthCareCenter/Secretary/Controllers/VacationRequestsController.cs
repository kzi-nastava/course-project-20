using HealthCareCenter.Service;
using HealthCareCenter.Model;
using System;
using System.Collections.ObjectModel;

namespace HealthCareCenter.Secretary.Controllers
{
    public class VacationRequestsController
    {
        public VacationRequestsController()
        {
            VacationRequestRepository.Load();
        }

        public ObservableCollection<VacationRequestDisplay> Get()
        {
            return VacationRequestService.Get();
        }

        public ObservableCollection<VacationRequestDisplay> Accept(int id)
        {
            return VacationRequestService.Accept(id);
        }

        public ObservableCollection<VacationRequestDisplay> Deny(int id, string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
            {
                throw new Exception("You must enter the reason for denial first.");
            }
            return VacationRequestService.Deny(id, reason);
        }
    }
}
