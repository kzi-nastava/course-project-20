using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Appointments.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Appointments.Controllers
{
    class AppointmentTermController
    {
        private readonly IAppointmentTermService _termService;

        public AppointmentTermController(IAppointmentTermService termService)
        {
            _termService = termService;
        }

        public List<AppointmentTerm> GetDailyTermsFromRange(int startHours, int startMinutes, int endHours, int endMinutes)
        {
            return _termService.GetDailyTermsFromRange(startHours, startMinutes, endHours, endMinutes);
        }

        public List<AppointmentTerm> GetDailyTermsFromRange(AppointmentTerm startRange, AppointmentTerm endRange)
        {
            return _termService.GetDailyTermsFromRange(startRange, endRange);
        }

        public List<AppointmentTerm> GetDailyTermsOppositeOfRange(int startHours, int startMinutes, int endHours, int endMinutes)
        {
            return _termService.GetDailyTermsFromRange(startHours, startMinutes, endHours, endMinutes);
        }

        public List<AppointmentTerm> GetDailyTermsOppositeOfRange(AppointmentTerm startRange, AppointmentTerm endRange)
        {
            return _termService.GetDailyTermsOppositeOfRange(startRange, endRange);
        }
    }
}
