using HealthCareCenter.Core.Rooms.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Rooms.Repositories
{
    public abstract class BaseRenovationScheduleRepository
    {
        public List<RenovationSchedule> Renovations { get; set; }

        public abstract int GetLargestId();

        protected abstract List<RenovationSchedule> Load();

        public abstract bool Save();
    }
}