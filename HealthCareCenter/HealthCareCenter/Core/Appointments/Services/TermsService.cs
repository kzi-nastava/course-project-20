using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Appointments.Repository;
using System;
using System.Collections.Generic;

namespace HealthCareCenter.Core.Appointments.Services
{
    public class TermsService : ITermsService
    {
        // returns all terms from 8:00 to 21:00 knowing that an appointment lasts 15 minutes
        // example { "8:00", "8:15", "8:30" ... "20:30", "20:45" }

        private readonly BaseAppointmentRepository _appointmentRepository;

        public TermsService(BaseAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public List<string> GetPossibleDailyTerms()
        {
            int hours = 8;
            int minutes = 0;
            List<string> possibleTerms = new List<string>();
            while (hours < 21)
            {
                string schedule = hours.ToString();
                if (minutes != 0)
                    schedule += ":" + minutes;
                else
                    schedule += ":" + minutes + "0";
                possibleTerms.Add(schedule);
                minutes += 15;
                if (minutes >= 60)
                {
                    ++hours;
                    minutes = 0;
                }
            }

            return possibleTerms;
        }

        public List<string> GetTermsWithinTwoHours()
        {
            List<string> possibleTerms = GetPossibleDailyTerms();
            List<string> termsWithinTwoHours = new List<string>();
            int currHour = DateTime.Now.Hour;
            int currMinute = DateTime.Now.Minute;
            foreach (string term in possibleTerms)
            {
                int termHour = int.Parse(term.Split(":")[0]);
                int termMinute = int.Parse(term.Split(":")[1]);
                int diff = termHour - currHour;

                bool withinTwoHours = diff == 1 || diff == 0 && termMinute >= currMinute || diff == 2 && termMinute <= currMinute;
                if (withinTwoHours)
                {
                    termsWithinTwoHours.Add(term);
                }
            }
            return termsWithinTwoHours;
        }

        public List<string> GetTermsAfterTwoHours(List<string> allPossibleTerms)
        {
            List<string> terms = new List<string>();
            foreach (string term in allPossibleTerms)
            {
                int hrs = int.Parse(term.Split(":")[0]);
                int mins = int.Parse(term.Split(":")[1]);
                if (hrs > DateTime.Now.Hour + 2 || hrs == DateTime.Now.Hour + 2 && mins > DateTime.Now.Minute)
                {
                    terms.Add(term);
                }
            }
            return terms;
        }

        public DateTime CreateTime(string term)
        {
            int termHour = int.Parse(term.Split(":")[0]);
            int termMinute = int.Parse(term.Split(":")[1]);
            DateTime time = DateTime.Now.Date.AddHours(termHour).AddMinutes(termMinute);
            return time;
        }

        public List<string> GetAvailableTerms(int doctorID, DateTime when)
        {
            List<string> terms = GetPossibleDailyTerms();
            foreach (Appointment appointment in _appointmentRepository.Appointments)
            {
                if (appointment.DoctorID != doctorID || appointment.ScheduledDate.Date.CompareTo(when) != 0)
                {
                    continue;
                }
                string unavailableTerm = appointment.ScheduledDate.Hour.ToString();
                if (appointment.ScheduledDate.Minute != 0)
                    unavailableTerm += ":" + appointment.ScheduledDate.Minute;
                else
                    unavailableTerm += ":" + appointment.ScheduledDate.Minute + "0";
                terms.Remove(unavailableTerm);
            }
            return terms;
        }
    }
}