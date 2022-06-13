using HealthCareCenter.Core.VacationRequests.Models;
using System;
using System.Collections.ObjectModel;

namespace HealthCareCenter.Core.VacationRequests.Services
{
    public interface IVacationRequestService
    {
        public bool OnVacation(int doctorID, DateTime when);
        public ObservableCollection<VacationRequestDisplay> Get();
        public ObservableCollection<VacationRequestDisplay> Accept(int id);
        public ObservableCollection<VacationRequestDisplay> Deny(int id, string reason);
    }
}
