using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter
{
    public class Doctor : User
    {
        public string Type { get; set; }
        public List<int> VacationRequestIDs { get; set; }
        public List<int> MedicineCreationRequestIDs { get; set; }
        public List<int> AllAppointmentIDs { get; set; }
    }
}