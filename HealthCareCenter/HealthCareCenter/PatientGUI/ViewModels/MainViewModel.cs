using HealthCareCenter.Model;
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
        private readonly Patient _patient;

        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        public ICommand ShowMyAppointments { get; }
        public ICommand ShowSearchForDoctors { get; }
        public ICommand ShowMyPrescriptions { get; }
        public ICommand ShowMyHealthRecord { get; }
        public ICommand ShowDoctorSurvey { get; }
        public ICommand ShowHealthCenterSurvey { get; }
        public ICommand LogOut { get; }

        public string CurrentViewLabel { get; set; }

        public MainViewModel(NavigationStore navigationStore, Patient patient)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

            _patient = patient;

            CurrentViewLabel = CurrentViewModel.ToString();
            OnPropertyChanged(nameof(CurrentViewLabel));

            ShowMyAppointments = new NavigateCommand(_navigationStore, ViewType.MyAppointments, _patient);
            ShowSearchForDoctors = new NavigateCommand(_navigationStore, ViewType.SearchDoctors, _patient);
            ShowMyPrescriptions = new NavigateCommand(_navigationStore, ViewType.MyPrescriptions, _patient);
            ShowMyHealthRecord = new NavigateCommand(_navigationStore, ViewType.MyHealthRecord, _patient);
            //ShowDoctorSurvey = new NavigateCommand(_navigationStore, ViewType.DoctorSurvey, _patient);
            //ShowHealthCenterSurvey = new NavigateCommand(_navigationStore, ViewType.HealthCenterSurvey, _patient);
            LogOut = new LogOutCommand();
        }

        private void OnCurrentViewModelChanged()
        {
            CurrentViewLabel = CurrentViewModel.ToString();
            OnPropertyChanged(nameof(CurrentViewLabel));
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
