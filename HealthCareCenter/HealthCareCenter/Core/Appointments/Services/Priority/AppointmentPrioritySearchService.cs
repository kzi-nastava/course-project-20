using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Users;
using HealthCareCenter.Core.Users.Models;
using System;
using System.Collections.Generic;

namespace HealthCareCenter.Core.Appointments.Services.Priority
{
    class AppointmentPrioritySearchService : IAppointmentPrioritySearchService
    {
        private readonly IPriorityAppointmentFinder _priorityFinder;
        private readonly ISimilarToPriorityAppointmentsFinder _similarPriorityFinder;
        private readonly IAppointmentTermService _termService;

        public AppointmentPrioritySearchService(
            IPriorityAppointmentFinder priorityFinder, 
            ISimilarToPriorityAppointmentsFinder similarPriorityFinder,
            IAppointmentTermService termService)
        {
            _priorityFinder = priorityFinder;
            _similarPriorityFinder = similarPriorityFinder;
            _termService = termService;
        }

        public Appointment GetPriorityAppointment(bool isDoctorPriority, int doctorID, int healthRecordID, DateTime finalScheduleDate, AppointmentTerm startRange, AppointmentTerm endRange)
        {
            Appointment newAppointment = isDoctorPriority
                ? _priorityFinder.GetDoctorPriorityAppointment(doctorID, healthRecordID, finalScheduleDate, startRange, endRange)
                : _priorityFinder.GetTimePriorityAppointment(doctorID, healthRecordID, finalScheduleDate, startRange, endRange);
            return newAppointment;
        }

        public List<Appointment> GetAppointmentsSimilarToPriorites(bool isDoctorPriority, int doctorID, int healthRecordID, DateTime finalScheduleDate, AppointmentTerm startRange, AppointmentTerm endRange)
        {
            List<Appointment> similarAppointments = new List<Appointment>();

            List<AppointmentTerm> possibleTerms = _termService.GetDailyTermsFromRange(startRange, endRange);
            List<AppointmentTerm> oppositePossibleTerms = _termService.GetDailyTermsOppositeOfRange(startRange, endRange);

            if (isDoctorPriority)
            {
                foreach (Doctor doctor in UserRepository.Doctors)
                {
                    similarAppointments.AddRange(_similarPriorityFinder.AppointmentsSimilarToPrioritySearch(doctor.ID, healthRecordID, finalScheduleDate, possibleTerms));
                    if (similarAppointments.Count >= 3)
                    {
                        break;
                    }
                }
            }
            else
            {
                similarAppointments.AddRange(_similarPriorityFinder.AppointmentsSimilarToPrioritySearch(doctorID, healthRecordID, finalScheduleDate, oppositePossibleTerms));
            }

            return similarAppointments;
        }

    }
}
