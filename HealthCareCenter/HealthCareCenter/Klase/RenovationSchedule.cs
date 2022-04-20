using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter
{
    public class RenovationSchedule
    {
<<<<<<< HEAD
        public DateTime _startDate { get; set; } // look again
        public DateTime _finishDate { get; set; } // look again
        public bool _isSpecialRenovation { get; set; } // look again => special
        public HospitalRoom _hospitalRooom { get; set; } // look again
        public HospitalRoom _secondaryRoom { get; set; } // look again!!! if is complex
=======
        public DateTime startDate { get; set; }
        public DateTime finishDate { get; set; }
        public bool isSpecialRenovation { get; set; }
        public HospitalRooom hospitalRooom { get; set; }
        public HospitalRooom secondaryRoom { get; set; } // if is complex
>>>>>>> 6ee85423f23a9fee7945477461b9c57c0719c897
    }
}