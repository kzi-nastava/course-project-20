using HealthCareCenter.Core.Appointments.Services;
using HealthCareCenter.GUI.Patient.AppointmentCRUD.ViewModels;
using HealthCareCenter.GUI.Patient.SharedCommands;
using HealthCareCenter.GUI.Patient.SharedViewModels;
using System.Windows;

namespace HealthCareCenter.GUI.Patient.DoctorSearch
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

            _navigationStore.CurrentViewModel = new AppointmentFormViewModel(
                _viewModel.Patient, 
                _navigationStore, 
                null, 
                false, 
                _viewModel.ChosenDoctor);

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
