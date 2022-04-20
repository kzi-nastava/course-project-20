using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Doctor
{
    class Doctor : User
    {
        public string type { get; set; }
        public List<VacationRequest> vacationRequests { get; set; }
        public List<MedicineCreationRequest> medicineSuggestions { get; set; }
        public List<Doctor> doctors { get; set; }
        public List<Appointment> allAppointments { get; set; }
    }
}
