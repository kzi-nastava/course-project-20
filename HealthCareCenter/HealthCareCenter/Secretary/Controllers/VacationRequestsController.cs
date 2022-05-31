using HealthCareCenter.Controller;
using HealthCareCenter.Enums;
using HealthCareCenter.Model;
using HealthCareCenter.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace HealthCareCenter.Secretary.Controllers
{
    public class VacationRequestsController
    {
        public VacationRequestsController()
        {
            VacationRequestRepository.Load();
        }

        public ObservableCollection<VacationRequestDisplay> Refresh()
        {
            return VacationRequestService.Refresh();
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
