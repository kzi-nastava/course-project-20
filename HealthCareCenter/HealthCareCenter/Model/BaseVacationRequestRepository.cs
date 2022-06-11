using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Model
{
    public abstract class BaseVacationRequestRepository
    {
        public List<VacationRequest> Requests { get; set; }
        public abstract void Load();
        public abstract void Save();
    }
}
