using HealthCareCenter.Model;
using HealthCareCenter.PatientGUI.ViewModels;
using HealthCareCenter.Service;
using System;
using System.Windows;

namespace HealthCareCenter.PatientGUI.Commands
{
    internal class ShowAnamnesisCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            if (_viewModel.ChosenAppointment == null)
            {
                MessageBox.Show("Appointment not chosen", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Appointment appointment = AppointmentService.Get(_viewModel.ChosenAppointment.AppointmentID);
            _viewModel.AnamnesisInfo = appointment.PatientAnamnesis.Comment;
        }

        private readonly MyHealthRecordViewModel _viewModel;

        public ShowAnamnesisCommand(MyHealthRecordViewModel viewModel)
        {
            _viewModel = viewModel;
        }
    }
}
