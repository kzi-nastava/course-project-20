using HealthCareCenter.Model;
using System.Collections.Generic;

namespace HealthCareCenter.Service
{
    class NotificationService
    {
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

        public static void Send(int userID, string message)
        {
            NotificationRepository.CalculateMaxID();
            Notification notification = new Notification(message, userID);
            NotificationRepository.Notifications.Add(notification);
            NotificationRepository.Save();
        }

        public static string Send(Appointment postponedAppointment, Appointment newAppointment, Patient patient)
        {
            string notificationToShowNow = null;
            HealthRecord postponedRecord = HealthRecordService.Get(postponedAppointment);

            NotificationRepository.CalculateMaxID();
            Notification postponedPatientNotification = new Notification($"The appointment you had scheduled at {newAppointment.ScheduledDate} has been postponed to {postponedAppointment.ScheduledDate}.", postponedRecord.PatientID);
            Notification postponedDoctorNotification = new Notification($"The appointment you had scheduled at {newAppointment.ScheduledDate} has been postponed to {postponedAppointment.ScheduledDate}.", postponedAppointment.DoctorID);
            Notification newDoctorNotification = new Notification($"A new urgent appointment has been scheduled for you at {newAppointment.ScheduledDate} in room {HospitalRoomService.Get(newAppointment.HospitalRoomID).Name}.", newAppointment.DoctorID);

            if (postponedRecord.PatientID == patient.ID)
            {
                postponedPatientNotification.Opened = true;
                notificationToShowNow = postponedPatientNotification.Message;
            }

            NotificationRepository.Notifications.AddRange(new Notification[] { postponedPatientNotification, postponedDoctorNotification, newDoctorNotification });
            NotificationRepository.Save();
            return notificationToShowNow;
        }
    }
}
