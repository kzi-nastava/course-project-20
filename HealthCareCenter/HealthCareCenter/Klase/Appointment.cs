using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Enums;

namespace HealthCareCenter
{
    public class Appointment
    {
        public int _id { get; set; }
        public AppointmentType _type { get; set; }
        public DateTime _appointmentDate { get; set; } // look again
        public DateTime dateScheduled { get; set; } // look again
        public bool _emergency { get; set; }
        public Doctor _doctor { get; set; }
        public HealthRecord _healthRecord { get; set; }
        public Anamnesis _anamnesis { get; set; }
        public HospitalRoom _hospitalRoom { get; set; }
    }
}