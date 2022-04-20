using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter
{
    public class Manager : User
    {
        public List<RenovationSchedule> renovationSchedules { get; set; }
        public List<MedicineCreationRequest> medicineSuggestions { get; set; }
        public List<Survey> surveys { get; set; }
    }
}