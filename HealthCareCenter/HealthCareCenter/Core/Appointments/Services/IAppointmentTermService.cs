using HealthCareCenter.Core.Appointments.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Appointments.Services
{
    interface IAppointmentTermService
    {
        public List<AppointmentTerm> GetDailyTermsFromRange(int startHours, int startMinutes, int endHours, int endMinutes);
        public List<AppointmentTerm> GetDailyTermsFromRange(AppointmentTerm startRange, AppointmentTerm endRange);
        public List<AppointmentTerm> GetDailyTermsOppositeOfRange(int startHours, int startMinutes, int endHours, int endMinutes);
        public List<AppointmentTerm> GetDailyTermsOppositeOfRange(AppointmentTerm startRange, AppointmentTerm endRange);
    }
}
