using HealthCareCenter.PatientGUI.Stores;
using HealthCareCenter.PatientGUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace HealthCareCenter.PatientGUI.Commands
{
    class SelectDoctorCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            if (_viewModel.ChosenDoctor == null)
            {
                MessageBox.Show("Doctor not chosen", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _navigationStore.CurrentViewModel = new CreateAppointmentViewModel(
                _navigationStore, _viewModel.Patient, _viewModel.ChosenDoctor);

        }

        private SearchDoctorsViewModel _viewModel;
        private NavigationStore _navigationStore;

        public SelectDoctorCommand(SearchDoctorsViewModel viewModel, NavigationStore navigationStore)
        {
            _viewModel = viewModel;
            _navigationStore = navigationStore;
        }
    }
}
