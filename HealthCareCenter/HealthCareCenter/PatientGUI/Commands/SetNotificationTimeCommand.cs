using HealthCareCenter.PatientGUI.ViewModels;
using System;
using System.Windows;

namespace HealthCareCenter.PatientGUI.Commands
{
    internal class SetNotificationTimeCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            if (string.IsNullOrEmpty(_viewModel.NotificationReceiveTime))
            {
                _ = MessageBox.Show("No time entered", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                int notificationReceiveTime = Convert.ToInt32(_viewModel.NotificationReceiveTime);
                if (notificationReceiveTime < 2 || notificationReceiveTime > 8)
                {
                    _ = MessageBox.Show("Invalid notification time, must be more than 2 and less than 8", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _viewModel.Patient.NotificationReceiveTime = notificationReceiveTime;
                _ = MessageBox.Show("Notification time set successfully", "My App", MessageBoxButton.OK, MessageBoxImage.Information);
                _viewModel.NotificationReceiveTime = "";
            }
            catch
            {
                _ = MessageBox.Show("Invalid notification time", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private readonly MyPrescriptionsViewModel _viewModel;

        public SetNotificationTimeCommand(MyPrescriptionsViewModel viewModel)
        {
            _viewModel = viewModel;
        }
    }
}
