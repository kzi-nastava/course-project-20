using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.VacationRequests.Models
{
    public class VacationRequestDisplay
    {
        public int ID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string RequestReason { get; set; }
        public string DoctorName { get; set; }
    }
}
