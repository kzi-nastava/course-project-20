using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter
{
    public class Manager : User
    {
        public List<RenovationSchedule> schedules { get; set; }
        public List<MedicineCreationRequest> medicineSuggestions { get; set; } // look again class name or field name
        public List<Survey> surveys { get; set; }
    }
}