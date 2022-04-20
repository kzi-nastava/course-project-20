using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter
{
    class VacationRequest
    {
        public Date startDate { get; set; }
        public Date endDate { get; set; }
        public string reason { get; set; }
        public bool emergency { get; set; }
        public RequestState requestState { get; set; }
    }
}
