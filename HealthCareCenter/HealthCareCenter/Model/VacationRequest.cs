using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Enums;

namespace HealthCareCenter.Model
{
    public class VacationRequest
    {
        public int ID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public bool Emergency { get; set; }
        public RequestState State { get; set; }
    }
}