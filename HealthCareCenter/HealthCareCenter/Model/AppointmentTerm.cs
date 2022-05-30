using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Model
{
    class AppointmentTerm
    {
        public int Hours { get; }
        public int Minutes { get; }

        public AppointmentTerm(int hours, int minutes)
        {
            Hours = hours;
            Minutes = minutes;
        }

        public AppointmentTerm(string time)
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

        public static bool operator <(AppointmentTerm ts1, AppointmentTerm ts2)
        {
            if (ts1.Hours < ts2.Hours)
            {
                return true;
            }
            else
            {
                return ts1.Hours == ts2.Hours && ts1.Minutes < ts2.Minutes;
            }
        }

        public static bool operator >(AppointmentTerm ts1, AppointmentTerm ts2)
        {
            return !(ts1 < ts2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (!(obj is AppointmentTerm))
            {
                return false;
            }
            return (Hours == ((AppointmentTerm)obj).Hours)
                && (Minutes == ((AppointmentTerm)obj).Minutes);
        }
    }
}
