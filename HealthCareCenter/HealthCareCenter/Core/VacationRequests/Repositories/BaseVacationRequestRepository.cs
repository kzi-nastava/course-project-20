using HealthCareCenter.Core.VacationRequests.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.VacationRequests.Repositories
{
    public abstract class BaseVacationRequestRepository
    {
        public List<VacationRequest> Requests { get; set; }
        public abstract void Load();
        public abstract void Save();
    }
}
