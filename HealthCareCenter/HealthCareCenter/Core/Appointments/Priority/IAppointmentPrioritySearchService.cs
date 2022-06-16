using HealthCareCenter.Core.Appointments.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Appointments.Services.Priority
{
    interface IAppointmentPrioritySearchService
    {
        public Appointment GetPriorityAppointment(bool isDoctorPriority, int doctorID, int healthRecordID, DateTime finalScheduleDate, AppointmentTerm startRange, AppointmentTerm endRange);
        public List<Appointment> GetAppointmentsSimilarToPriorites(bool isDoctorPriority, int doctorID, int healthRecordID, DateTime finalScheduleDate, AppointmentTerm startRange, AppointmentTerm endRange);
    }
}
