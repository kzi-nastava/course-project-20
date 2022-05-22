using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Model;

namespace HealthCareCenter.Service
{
    class AppointmentTermService
    {
        public static List<AppointmentTerm> GetDailyTermsFromRange(int startHours, int startMinutes, int endHours, int endMinutes)
        {
            int hours = startHours;
            int minutes = startMinutes;
            List<AppointmentTerm> possibleSchedules = new List<AppointmentTerm>();
            while (hours < endHours || minutes < endMinutes)
            {
                possibleSchedules.Add(new AppointmentTerm(hours, minutes));
                minutes += 15;
                if (minutes >= 60)
                {
                    ++hours;
                    minutes = 0;
                }
            }

            return possibleSchedules;
        }

        public static List<AppointmentTerm> GetDailyTermsFromRange(AppointmentTerm startRange, AppointmentTerm endRange)
        {
            return GetDailyTermsFromRange(startRange.Hours, startRange.Minutes, endRange.Hours, endRange.Minutes);
        }

        public static List<AppointmentTerm> GetDailyTermsOppositeOfRange(int startHours, int startMinutes, int endHours, int endMinutes)
        {
            List<AppointmentTerm> possibleSchedules;

            if (startHours == Constants.StartWorkTime && startMinutes == 0)
            {
                possibleSchedules = GetDailyTermsFromRange(endHours, endHours, Constants.EndWorkTime, 0);
            }
            else
            {
                if (endHours == Constants.EndWorkTime && endMinutes == 0)
                {
                    possibleSchedules = GetDailyTermsFromRange(Constants.StartWorkTime, 0, startHours, startMinutes);
                }
                else
                {
                    List<AppointmentTerm> beforeStartRangeSchedules = GetDailyTermsFromRange(Constants.StartWorkTime, 0, startHours, startMinutes);
                    List<AppointmentTerm> afterStartRangeSchedules = GetDailyTermsFromRange(endHours, endMinutes, Constants.EndWorkTime, 0);
                    possibleSchedules = beforeStartRangeSchedules;
                    possibleSchedules.AddRange(afterStartRangeSchedules);
                }
            }

            return possibleSchedules;
        }

        public static List<AppointmentTerm> GetDailyTermsOppositeOfRange(AppointmentTerm startRange, AppointmentTerm endRange)
        {
            return GetDailyTermsOppositeOfRange(startRange.Hours, startRange.Minutes, endRange.Hours, endRange.Minutes);
        }
    }
}
