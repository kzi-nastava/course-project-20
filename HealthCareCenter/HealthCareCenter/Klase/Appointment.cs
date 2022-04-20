using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter
{
    class Appointment
    {
        public AppointmentType type { get; set; }
        public Date appointmentDate { get; set; }
        public Date dateScheduled { get; set; }
        public int id { get; set; }
        public bool emergency { get; set; }
        public Doctor doctor { get; set; }
        public HealthRecord healthRecord { get; set; }
        public Anamnesis anamnesis { get; set; }
        public HospitalRoom hospitalRoom { get; set; }
    }
}
