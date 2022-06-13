using HealthCareCenter.Core.Notifications;
using HealthCareCenter.Core.Notifications.Services;
using HealthCareCenter.Core.Users.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Users
{
    public class SecretaryController
    {
        private readonly INotificationService _notificationService;

        public SecretaryController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public List<Notification> GetNotifications(User user)
        {
            List<Notification> notifications = _notificationService.GetUnopened(user);
            if (notifications.Count == 0)
            {
                throw new Exception("No new notifications to show.");
            }
            return notifications;
        }
    }
}
