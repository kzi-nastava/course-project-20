using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter
{
    internal class RenovationSchedule
    {
        private DateTime startDate;
        private DateTime finishDate;
        private bool isSpecialRenovation;
        private HospitalRooom hospitalRooom;
        private HospitalRooom secondaryRoom; // if is complex
    }
}