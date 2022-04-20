using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter
{
    public class RenovationSchedule
    {
        public DateTime startDate { get; set; }
        public DateTime finishDate { get; set; }
        public bool isSpecialRenovation { get; set; }
        public HospitalRooom hospitalRooom { get; set; }
        public HospitalRooom secondaryRoom { get; set; } // if is complex
    }
}