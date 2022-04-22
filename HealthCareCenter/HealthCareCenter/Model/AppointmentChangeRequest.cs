using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Enums;

namespace HealthCareCenter
{
    public class AppointmentChangeRequest
    {
        public int ID { get; set; }
        public RequestType NewRequestType { get; set; }
        public DateTime NewDate { get; set; }
        public AppointmentType NewAppointmentType { get; set; }
        public int NewDoctorID { get; set; }
        public DateTime DateSent { get; set; }
        public int PatientID { get; set; }
    }
}