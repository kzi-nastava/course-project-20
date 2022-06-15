using HealthCareCenter.Core.Appointments.Models;
using System.Collections.Generic;

namespace HealthCareCenter.Core.Appointments.Repository
{
    public abstract class BaseAppointmentChangeRequestRepository
    {
        protected List<AppointmentChangeRequest> _requests;
        public List<AppointmentChangeRequest> Requests
        {
            get
            {
                if (_requests == null)
                {
                    _ = Load();
                }
                return _requests;
            }
        }
        public int LargestID { get; set; }

        public abstract List<AppointmentChangeRequest> Load();
        public abstract void Save();
        public abstract int GetLargestID();
    }
}
