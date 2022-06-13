using HealthCareCenter.Core.VacationRequests.Models;
using System;
using System.Collections.ObjectModel;

namespace HealthCareCenter.Core.VacationRequests.Services
{
    public interface IVacationRequestService
    {
        public bool OnVacation(int doctorID, DateTime when);
        public ObservableCollection<VacationRequestForDisplay> Get();
        public ObservableCollection<VacationRequestForDisplay> Accept(int id);
        public ObservableCollection<VacationRequestForDisplay> Deny(int id, string reason);
    }
}
