using HealthCareCenter.Core.Appointments.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Appointments.Repository
{
    public abstract class BaseAppointmentRepository
    {
        protected List<Appointment> _appointments;
        public List<Appointment> Appointments
        {
            get
            {
                if (_appointments == null)
                {
                    _ = Load();
                }
                return _appointments;
            }
        }
        public static int LargestID { get; set; }

        public abstract List<Appointment> Load();
        public abstract void Save();
        public abstract int GetLargestID();
    }
}
