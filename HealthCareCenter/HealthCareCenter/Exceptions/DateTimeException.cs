using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Exceptions
{
    public class InvalideDateException : Exception
    {
        public InvalideDateException(string dateTime) : base($"Invalide date={dateTime}!")
        {
        }
    }

    public class DateIsBeforeTodaException : Exception
    {
        public DateIsBeforeTodaException(string dateTime) : base($"Date={dateTime} is before today!")
        {
        }
    }
}