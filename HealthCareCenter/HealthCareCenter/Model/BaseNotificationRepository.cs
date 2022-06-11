using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Model
{
    public abstract class BaseNotificationRepository
    {
        public List<Notification> Notifications { get; set; }
        public int maxID = -1;
        public abstract void CalculateMaxID();
        public abstract void Load();
        public abstract void Save();
    }
}
