using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Model
{
    class NotificationRepository
    {
        public static List<Notification> Notifications { get; set; }
        public static int maxID = -1;

        public static void CalculateMaxID()
        {
            maxID = -1;
            foreach (Notification notification in Notifications)
            {
                if (notification.ID > maxID)
                {
                    maxID = notification.ID;
                }
            }
        }

        public static void Load()
        {
            try
            {
                string JSONTextNotifications = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\notifications.json");
                Notifications = (List<Notification>)JsonConvert.DeserializeObject<IEnumerable<Notification>>(JSONTextNotifications);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Save()
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer
                {
                    Formatting = Formatting.Indented
                };

                using (StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\notifications.json"))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, Notifications);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
