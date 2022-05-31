using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Secretary
{
    /// <summary>
    /// Class used only for displaying delete appointment requests of a specific patient in ViewChangeRequestsWindow
    /// </summary>
    public class DeleteRequest
    {
        public int ID { get; set; }
        public DateTime TimeSent { get; set; }
        public string DoctorUsername { get; set; }
        public DateTime AppointmentTime { get; set; }

        public DeleteRequest() { }
        public DeleteRequest(int id, DateTime timeSent)
        {
            ID = id;
            TimeSent = timeSent;
        }
    }
}
