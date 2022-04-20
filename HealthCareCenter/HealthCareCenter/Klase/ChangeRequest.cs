using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter
{
    class ChangeRequest
    {
        public RequestType requestType { get; set; }
        public Date newDate { get; set; }
        public AppointmentType newType { get; set; }
        public Doctor newDoctor { get; set; }
        public Date dateSent { get; set; }
        public int appointmentId { get; set; }
        public Patient patient { get; set; }
    }
}
