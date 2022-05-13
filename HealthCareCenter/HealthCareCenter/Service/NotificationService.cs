using HealthCareCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Service
{
    class NotificationService
    {
        public static int maxID = -1;

        public static void CalculateMaxID()
        {
            maxID = -1;
            foreach (Notification notification in NotificationRepository.Notifications)
            {
                if (notification.ID > maxID)
                {
                    maxID = notification.ID;
                }
            }
        }

        public static List<Notification> FindUnopened(User user)
        {
            List<Notification> notifications = new List<Notification>();
            foreach (Notification notification in NotificationRepository.Notifications)
            {
                if (notification.UserID == user.ID && !notification.Opened)
                {
                    notification.Opened = true;
                    notifications.Add(notification);
                }
            }
            NotificationRepository.Save();
            return notifications;
        }
    }
}
