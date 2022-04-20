using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter
{
    internal class Manager : User
    {
        private List<RenovationSchedule> _renovationSchedules;
        private List<MedicineCreationRequest> medicineSuggestions;
        private List<Survey> surveys;
    }
}