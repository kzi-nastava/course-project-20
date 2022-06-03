﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HealthCareCenter.Model;
using HealthCareCenter.Service;

namespace HealthCareCenter.SecretaryGUI
{
    public partial class SecretaryWindow : Window
    {
        private Secretary _signedUser;
        public SecretaryWindow(User user)
        {
            _signedUser = (Secretary)user;
            InitializeComponent();
            DisplayNotifications();
        }

        private void DisplayNotifications()
        {
            List<Notification> notifications = NotificationService.FindUnopened(_signedUser);
            if (notifications.Count == 0)
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
            DistributeDynamicEquipmentWindow window = new DistributeDynamicEquipmentWindow();
            window.ShowDialog();
        }
        
        private void EquipmentAcquisitionButton_Click(object sender, RoutedEventArgs e)
        {
            DynamicEquipmentRequestWindow window = new DynamicEquipmentRequestWindow(_signedUser);
            window.ShowDialog();
        }

        private void VacationButton_Click(object sender, RoutedEventArgs e)
        {
            VacationRequestsWindow window = new VacationRequestsWindow();
            window.ShowDialog();
        }
    }
}
