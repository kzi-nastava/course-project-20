using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Enums;

namespace HealthCareCenter.Model
{
    public class Appointment
    {
        public int ID { get; set; }
        public AppointmentType Type { get; set; }
        public DateTime AppointmentDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Emergency { get; set; }
        public int DoctorID { get; set; }
        public int HealthRecordID { get; set; }
        public int HospitalRoomID { get; set; }
        public Anamnesis PatientAnamnesis { get; set; }
    }
}