using HealthCareCenter.PatientGUI.Models;
using HealthCareCenter.PatientGUI.Stores;
using HealthCareCenter.PatientGUI.ViewModels;
using System;
using System.Windows;

namespace HealthCareCenter.PatientGUI.Commands
{
    internal class CancelAppointmentCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            if (_viewModel.ChosenAppointment == null)
            {
                MessageBox.Show("Appointment not chosen", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            PatientFunctionality patFunc = PatientFunctionality.GetInstance();
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure?", "Schedule appointment?", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                DateTime oldScheduleDate = Convert.ToDateTime(_viewModel.ChosenAppointment.AppointmentDate);
                if (patFunc.ShouldSendToSecretary(oldScheduleDate))
                {
                    _ = MessageBox.Show("Since there are less than 2 days until this appointment starts, a request will be sent to the secretary",
                        "My App", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                patFunc.CancelAppointment(
                    Convert.ToInt32(_viewModel.ChosenAppointment.AppointmentID), _viewModel.Patient.ID,
                    Convert.ToDateTime(_viewModel.ChosenAppointment.AppointmentDate));

                _navigationStore.CurrentViewModel = new MyAppointmentsViewModel(_navigationStore, _viewModel.Patient);
            }
        }

        private readonly MyAppointmentsViewModel _viewModel;
        private readonly NavigationStore _navigationStore;

        public CancelAppointmentCommand(MyAppointmentsViewModel viewModel, NavigationStore navigationStore)
        {
            _viewModel = viewModel;
            _navigationStore = navigationStore;
        }
    }
}
