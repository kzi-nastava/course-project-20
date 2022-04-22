using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Model
{
    public class RenovationSchedule
    {
        public int ID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public bool IsSpecialRenovation { get; set; } // special
        public int PrimaryRoomID { get; set; }
        public int SecondaryRoomID { get; set; } // if is complex
    }
}