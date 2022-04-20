using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Enums;

namespace HealthCareCenter
{
    public class ChangeRequest
    {
        public RequestType _requestType { get; set; }
        public DateTime _newDate { get; set; }
        public AppointmentType _newType { get; set; }
        public Doctor _newDoctor { get; set; }
        public DateTime _dateSent { get; set; } // look again
        public int _id { get; set; }
        public Patient _patient { get; set; }
    }
}