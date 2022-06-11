using HealthCareCenter.Model;
using HealthCareCenter.Secretary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace HealthCareCenter.Service
{
    public interface IVacationRequestService
    {
        public bool OnVacation(int doctorID, DateTime when);
        public ObservableCollection<VacationRequestDisplay> Get();
        public ObservableCollection<VacationRequestDisplay> Accept(int id);
        public ObservableCollection<VacationRequestDisplay> Deny(int id, string reason);
    }
}
