using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Appointments.Services.Priority;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Appointments.Controllers
{
    class AppointmentPrioritySearchController
    {
        private readonly IAppointmentPrioritySearchService _prioritySearchService;

        public AppointmentPrioritySearchController(IAppointmentPrioritySearchService prioritySearchService)
        {
            _prioritySearchService = prioritySearchService;
        }

        public Appointment GetPriorityAppointment(bool isDoctorPriority, int doctorID, int healthRecordID, DateTime finalScheduleDate, AppointmentTerm startRange, AppointmentTerm endRange)
        {
            return _prioritySearchService.GetPriorityAppointment(isDoctorPriority, doctorID, healthRecordID, finalScheduleDate, startRange, endRange);
        }

        public List<Appointment> GetAppointmentsSimilarToPriorites(bool isDoctorPriority, int doctorID, int healthRecordID, DateTime finalScheduleDate, AppointmentTerm startRange, AppointmentTerm endRange)
        {
            return _prioritySearchService.GetAppointmentsSimilarToPriorites(isDoctorPriority, doctorID, healthRecordID, finalScheduleDate, startRange, endRange);
        }
    }
}
