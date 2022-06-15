using HealthCareCenter.Core.Appointments.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Appointments.Services.Priority
{
    interface ISimilarToPriorityAppointmentsFinder
    {
        public List<Appointment> AppointmentsSimilarToPrioritySearch(int doctorID, int healthRecordID, DateTime finalScheduleDate, List<AppointmentTerm> possibleTerms);
    }
}
