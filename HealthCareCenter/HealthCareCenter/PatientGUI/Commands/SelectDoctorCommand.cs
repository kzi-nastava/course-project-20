using HealthCareCenter.PatientGUI.Stores;
using HealthCareCenter.PatientGUI.ViewModels;
using System.Windows;

namespace HealthCareCenter.PatientGUI.Commands
{
    internal class SelectDoctorCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            if (_viewModel.ChosenDoctor == null)
            {
                _ = MessageBox.Show("Doctor not chosen", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _navigationStore.CurrentViewModel = new CreateAppointmentViewModel(
                _navigationStore, _viewModel.Patient, _viewModel.ChosenDoctor);

        }

        private readonly SearchDoctorsViewModel _viewModel;
        private readonly NavigationStore _navigationStore;

        public SelectDoctorCommand(SearchDoctorsViewModel viewModel, NavigationStore navigationStore)
        {
            _viewModel = viewModel;
            _navigationStore = navigationStore;
        }
    }
}
