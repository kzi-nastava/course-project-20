using HealthCareCenter.Model;
using HealthCareCenter.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Secretary.Controllers
{
    public class SecretaryController
    {
        public List<Notification> GetNotifications(User user)
        {
            List<Notification> notifications = NotificationService.GetUnopened(user);
            if (notifications.Count == 0)
            {
                throw new Exception("No new notifications to show.");
            }
            return notifications;
        }
    }
}
