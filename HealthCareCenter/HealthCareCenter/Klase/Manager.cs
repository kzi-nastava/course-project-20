using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter
{
    public class Manager : User
    {
<<<<<<< HEAD
        public List<RenovationSchedule> schedules { get; set; }
        public List<MedicineCreationRequest> medicineSuggestions { get; set; } // look again class name or field name
=======
        public List<RenovationSchedule> renovationSchedules { get; set; }
        public List<MedicineCreationRequest> medicineSuggestions { get; set; }
>>>>>>> 6ee85423f23a9fee7945477461b9c57c0719c897
        public List<Survey> surveys { get; set; }
    }
}