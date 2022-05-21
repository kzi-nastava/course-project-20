using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Model;

namespace HealthCareCenter.Service
{
    class TimeScheduleService
    {
        public static List<TimeSchedule> GetDailySchedulesFromRange(int startHours, int startMinutes, int endHours, int endMinutes)
        {
            int hours = startHours;
            int minutes = startMinutes;
            List<TimeSchedule> possibleSchedules = new List<TimeSchedule>();
            while (hours < endHours || minutes < endMinutes)
            {
                string schedule = hours + ":" + minutes;
                possibleSchedules.Add(new TimeSchedule(hours, minutes));
                minutes += 15;
                if (minutes >= 60)
                {
                    ++hours;
                    minutes = 0;
                }
            }

            return possibleSchedules;
        }

        public static List<TimeSchedule> GetDailySchedulesFromRange(TimeSchedule startRange, TimeSchedule endRange)
        {
            return GetDailySchedulesFromRange(startRange.Hours, startRange.Minutes, endRange.Hours, endRange.Minutes);
        }

        public static List<TimeSchedule> GetDailySchedulesOppositeOfRange(int startHours, int startMinutes, int endHours, int endMinutes)
        {
            List<TimeSchedule> possibleSchedules;

            if (startHours == Constants.StartWorkTime && startMinutes == 0)
            {
                possibleSchedules = GetDailySchedulesFromRange(endHours, endHours, Constants.EndWorkTime, 0);
            }
            else
            {
                if (endHours == Constants.EndWorkTime && endMinutes == 0)
                {
                    possibleSchedules = GetDailySchedulesFromRange(Constants.StartWorkTime, 0, startHours, startMinutes);
                }
                else
                {
                    List<TimeSchedule> beforeStartRangeSchedules = GetDailySchedulesFromRange(Constants.StartWorkTime, 0, startHours, startMinutes);
                    List<TimeSchedule> afterStartRangeSchedules = GetDailySchedulesFromRange(endHours, endMinutes, Constants.EndWorkTime, 0);
                    possibleSchedules = beforeStartRangeSchedules;
                    possibleSchedules.AddRange(afterStartRangeSchedules);
                }
            }

            return possibleSchedules;
        }

        public static List<TimeSchedule> GetDailySchedulesOppositeOfRange(TimeSchedule startRange, TimeSchedule endRange)
        {
            return GetDailySchedulesOppositeOfRange(startRange.Hours, startRange.Minutes, endRange.Hours, endRange.Minutes);
        }
    }
}
