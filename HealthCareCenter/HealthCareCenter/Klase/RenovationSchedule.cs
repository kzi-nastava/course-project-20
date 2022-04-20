using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter
{
    public class RenovationSchedule
    {
        public DateTime _startDate { get; set; } // look again
        public DateTime _finishDate { get; set; } // look again
        public bool _isSpecialRenovation { get; set; } // look again => special
        public HospitalRoom _hospitalRooom { get; set; } // look again
        public HospitalRoom _secondaryRoom { get; set; } // look again!!! if is complex
    }
}