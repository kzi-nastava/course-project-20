using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Exceptions
{
    public class InvalideDateException : Exception
    {
        public InvalideDateException(string dateTime) : base($"Invalide date={dateTime}!")
        {
        }
    }

    public class DateIsBeforeTodayException : Exception
    {
        public DateIsBeforeTodayException(string dateTime) : base($"Date={dateTime} is before today!")
        {
        }
    }
}