using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Model
{
    public static class Utils
    {
        public static List<string> GetPossibleDailyTerms()
        {
            // returns all terms from 8:00 to 21:00 knowing that an appointment lasts 15 minutes
            // example { "8:00", "8:15", "8:30" ... "20:30", "20:45" }

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
    }
}
