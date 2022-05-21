using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Model
{
    class TimeSchedule
    {
        public int Hours { get; }
        public int Minutes { get; }

        public TimeSchedule(int hours, int minutes)
        {
            Hours = hours;
            Minutes = minutes;
        }

        public TimeSchedule(string time)
        {
            string[] hoursAndMinutes = time.Split(":");

            Hours = Convert.ToInt32(hoursAndMinutes[0]);
            Minutes = Convert.ToInt32(hoursAndMinutes[1]);
        }

        public override string ToString()
        {
            string time = (Hours < 10) ? "0" + Hours + ":" : Hours + ":";
            time = (Minutes < 10) ? time + "0" + Minutes : time + Minutes;
            return time;
        }

        public static bool operator <(TimeSchedule ts1, TimeSchedule ts2)
        {
            if (ts1.Hours < ts2.Hours)
            {
                return true;
            }
            else
            {
                if (ts1.Hours == ts2.Hours && ts1.Minutes < ts2.Minutes)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool operator >(TimeSchedule ts1, TimeSchedule ts2)
        {
            if (ts1.Hours > ts2.Hours)
            {
                return true;
            }
            else
            {
                if (ts1.Hours == ts2.Hours && ts1.Minutes > ts2.Minutes)
                {
                    return true;
                }
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (!(obj is TimeSchedule))
            {
                return false;
            }
            return (Hours == ((TimeSchedule)obj).Hours)
                && (Minutes == ((TimeSchedule)obj).Minutes);
        }
    }
}
