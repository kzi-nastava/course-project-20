using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Appointments.Models
{
    public class AppointmentChangeRequest
    {
        public int ID { get; set; }
        public int AppointmentID { get; set; }
        public RequestType RequestType { get; set; }
        public RequestState State { get; set; }
        public DateTime NewDate { get; set; }
        public AppointmentType NewAppointmentType { get; set; }
        public int NewDoctorID { get; set; }
        public DateTime DateSent { get; set; }
        public int PatientID { get; set; }
    }
}