using System;
using System.Collections.Generic;
using System.Windows;
using HealthCareCenter.Model;
using HealthCareCenter.Secretary.Controllers;
using HealthCareCenter.Service;

namespace HealthCareCenter.Secretary
{
    public partial class SecretaryWindow : Window
    {
        private readonly Model.Secretary _signedUser;
        private readonly SecretaryController _controller;
        private readonly IDynamicEquipmentService _dynamicEquipmentService;
        private readonly INotificationService _notificationService;

        public SecretaryWindow(User user, INotificationService notificationService, IDynamicEquipmentService dynamicEquipmentService)
        {
            _signedUser = (Model.Secretary)user;
            _controller = new SecretaryController(notificationService);
            _dynamicEquipmentService = dynamicEquipmentService;
            _notificationService = notificationService;

            InitializeComponent();
            DisplayNotifications();
        }

        private void DisplayNotifications()
        {
            List<Notification> notifications;
            try
            {
                notifications = _controller.GetNotifications(_signedUser);
            }
            catch (Exception)
            {
                return;
            }
            MessageBox.Show("You have new notifications.");
            foreach (Notification notification in notifications)
            {
                MessageBox.Show(notification.Message);
            }
        }

        private void PatientButton_Click(object sender, RoutedEventArgs e)
        {
            PatientManipulationWindow window = new PatientManipulationWindow();
            window.ShowDialog();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            LoginWindow window = new LoginWindow();
            window.Show();
        }

        private void EquipmentDistributionButton_Click(object sender, RoutedEventArgs e)
        {
            DistributeDynamicEquipmentWindow window = new DistributeDynamicEquipmentWindow(_dynamicEquipmentService);
            window.ShowDialog();
        }
        
        private void EquipmentAcquisitionButton_Click(object sender, RoutedEventArgs e)
        {
            DynamicEquipmentRequestWindow window = new DynamicEquipmentRequestWindow(_signedUser, _dynamicEquipmentService);
            window.ShowDialog();
        }

        private void VacationButton_Click(object sender, RoutedEventArgs e)
        {
            VacationRequestsWindow window = new VacationRequestsWindow(_notificationService);
            window.ShowDialog();
        }
    }
}
