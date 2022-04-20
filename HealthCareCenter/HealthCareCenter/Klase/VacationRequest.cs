using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter
{
    public class VacationRequest
    {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string reason { get; set; }
        public bool emergency { get; set; }
        public RequestState requestState { get; set; }
    }
}
