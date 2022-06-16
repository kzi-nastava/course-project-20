using HealthCareCenter.Core.Appointments.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Appointments.Services.Priority
{
    interface IPriorityAppointmentFinder
    {
        public Appointment GetDoctorPriorityAppointment(int doctorID, int healthRecordID, DateTime finalScheduleDate, AppointmentTerm startRange, AppointmentTerm endRange);
        public Appointment GetTimePriorityAppointment(int doctorID, int healthRecordID, DateTime finalScheduleDate, AppointmentTerm startRange, AppointmentTerm endRange);
        public Appointment PrioritySearch(int doctorID, DateTime finalScheduleDate, int healthRecordID, List<AppointmentTerm> possibleTerms);
        public Appointment BothPrioritiesSearch(int doctorID, int healthRecordID, DateTime finalScheduleDate, AppointmentTerm startRange, AppointmentTerm endRange);
        public Appointment DifferentDoctorSameTimeSearch(int doctorID, int healthRecordID, DateTime finalScheduleDate, AppointmentTerm startRange, AppointmentTerm endRange);
        public Appointment SameDoctorDifferentTimeSearch(int doctorID, int healthRecordID, DateTime finalScheduleDate, AppointmentTerm startRange, AppointmentTerm endRange);
    }
}
