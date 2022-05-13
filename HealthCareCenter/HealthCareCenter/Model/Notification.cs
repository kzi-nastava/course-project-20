using HealthCareCenter.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Model
{
    class Notification
    {
        public int ID { get; set; }
        public string Message { get; set; }
        public bool Opened { get; set; }
        public int UserID { get; set; }

        public Notification() { }
        public Notification(string message, int userID)
        {
            ID = ++NotificationService.maxID;
            Message = message;
            Opened = false;
            UserID = userID;
        }
    }
}
