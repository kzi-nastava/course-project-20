using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Rooms.Models
{
    /// <summary>
    /// Class used only for displaying available hospital rooms in ScheduleAppointmentReferralWindow
    /// </summary>
    public class HospitalRoomDisplay
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
