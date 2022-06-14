using HealthCareCenter.GUI.Patient.AppointmentCRUD.ViewModels;
using HealthCareCenter.GUI.Patient.SharedCommands;
using HealthCareCenter.GUI.Patient.SharedViewModels;
using System.Windows;

namespace HealthCareCenter.GUI.Patient.AppointmentCRUD.Commands
{
    internal class ShowAppointmentFormCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            if (_isModification && _viewModel.ChosenAppointment == null)
            {
                _ = MessageBox.Show("Appointment not chosen", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _navigationStore.CurrentViewModel = new AppointmentFormViewModel(
                _viewModel.Patient, 
                _navigationStore, 
                _viewModel.ChosenAppointment, 
                _isModification, null);
        }

        private readonly MyAppointmentsViewModel _viewModel;
        private readonly NavigationStore _navigationStore;
        private readonly bool _isModification;

        public ShowAppointmentFormCommand(MyAppointmentsViewModel viewModel, NavigationStore navigationStore, bool isModification)
        {
            _viewModel = viewModel;
            _navigationStore = navigationStore;
            _isModification = isModification;
        }
    }
}
