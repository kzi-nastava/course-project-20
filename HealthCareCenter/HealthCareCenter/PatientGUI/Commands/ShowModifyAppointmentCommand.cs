using HealthCareCenter.PatientGUI.Stores;
using HealthCareCenter.PatientGUI.ViewModels;
using System.Windows;

namespace HealthCareCenter.PatientGUI.Commands
{
    internal class ShowModifyAppointmentCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            if (_viewModel.ChosenAppointment == null)
            {
                _ = MessageBox.Show("Appointment not chosen", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _navigationStore.CurrentViewModel = new ModifyAppointmentViewModel(_navigationStore, _viewModel.Patient, _viewModel.ChosenAppointment);
        }

        private readonly MyAppointmentsViewModel _viewModel;
        private readonly NavigationStore _navigationStore;

        public ShowModifyAppointmentCommand(MyAppointmentsViewModel viewModel, NavigationStore navigationStore)
        {
            _viewModel = viewModel;
            _navigationStore = navigationStore;
        }
    }
}
