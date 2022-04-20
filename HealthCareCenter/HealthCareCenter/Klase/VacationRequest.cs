using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Enums;

namespace HealthCareCenter
{
    public class VacationRequest
    {
        public DateTime _startDate { get; set; } // look again
        public DateTime _endDate { get; set; } // look again
        public string _reason { get; set; }
        public bool _emergency { get; set; }
        public RequestState _requestState { get; set; } // look again
    }
}