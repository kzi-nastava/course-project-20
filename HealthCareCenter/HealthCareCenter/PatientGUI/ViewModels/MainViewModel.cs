using HealthCareCenter.PatientGUI.Commands;
using HealthCareCenter.PatientGUI.Models;
using HealthCareCenter.PatientGUI.Stores;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace HealthCareCenter.PatientGUI.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        public ICommand ShowMyAppointments { get; }
        public ICommand ShowSearchForDoctor { get; }
        public ICommand ShowMyPrescriptions { get; }
        public ICommand ShowMyHealthRecord { get; }
        public ICommand ShowDoctorSurvey { get; }
        public ICommand ShowHealthCenterSurvey { get; }

        public MainViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;

            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

            ShowMyAppointments = new NavigateCommand(_navigationStore, ViewType.MyAppointments);
            // instantiate remaining commands
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
