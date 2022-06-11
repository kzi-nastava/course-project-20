using HealthCareCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Service
{
    public interface INotificationService
    {
        List<Notification> GetUnopened(User user);
        void Send(int userID, string message);
        Dictionary<int, Dictionary<int, int>> GetNotificationsSentDict(List<Prescription> patientPrescriptions);
        string Send(Appointment postponedAppointment, Appointment newAppointment, Patient patient);
        List<string> GetNotifications(Dictionary<int, Dictionary<int, int>> notificationsFromPrescriptionsToSend, Patient patient);
    }
}
