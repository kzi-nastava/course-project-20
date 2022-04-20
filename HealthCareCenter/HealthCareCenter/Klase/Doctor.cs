using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter
{
    public class Doctor : User
    {
        public string _type { get; set; }
        public List<VacationRequest> _vacationRequests { get; set; } // look again => _requests
        public List<MedicineCreationRequest> medicineSuggestions { get; set; } // look again
        public List<Doctor> _doctors { get; set; } // look again
        public List<Appointment> _allAppointments { get; set; } // look again _appoinments
    }
}