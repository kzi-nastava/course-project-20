﻿using HealthCareCenter.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.SecretaryGUI
{
    /// <summary>
    /// Class used only for displaying edit appointment requests of a specific patient in ViewChangeRequestsWindow
    /// </summary>
    public class EditRequest
    {
        public int ID { get; set; }
        public DateTime TimeSent { get; set; }
        public string OriginalDoctorUsername { get; set; }
        public string NewDoctorUsername { get; set; }
        public DateTime OriginalAppointmentTime { get; set; }
        public DateTime NewAppointmentTime { get; set; }
        public AppointmentType OriginalType { get; set; }
        public AppointmentType NewType { get; set; }

        public EditRequest() { }

        public EditRequest(int id, DateTime timeSent)
        {
            ID = id;
            TimeSent = timeSent;
        }
    }
}
