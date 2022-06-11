using HealthCareCenter.Model;
using HealthCareCenter.PatientGUI.Commands;
using HealthCareCenter.PatientGUI.Models;
using HealthCareCenter.PatientGUI.Stores;
using HealthCareCenter.Service;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace HealthCareCenter.PatientGUI.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly Patient _patient;
        private INotificationService _notificationService;


        private readonly Dictionary<int, Dictionary<int, int>> _notificationsFromPrescriptionsToSend;

        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        public ICommand ShowMyAppointments { get; }
        public ICommand ShowSearchForDoctors { get; }
        public ICommand ShowMyPrescriptions { get; }
        public ICommand ShowMyHealthRecord { get; }
        public ICommand ShowDoctorSurvey { get; }
        public ICommand ShowHealthCenterSurvey { get; }
        public ICommand LogOut { get; }

        public string CurrentViewLabel { get; set; }

        public MainViewModel(NavigationStore navigationStore, Patient patient, INotificationService notificationService)
        {
            _notificationService = notificationService;
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

            _patient = patient;

            _notificationsFromPrescriptionsToSend = _notificationService.GetNotificationsSentDict(
                PrescriptionService.GetPatientPrescriptions(_patient.HealthRecordID));
            ShowNotifications();  // show notifications on startup

            CurrentViewLabel = CurrentViewModel.ToString();
            OnPropertyChanged(nameof(CurrentViewLabel));
            StartNotificationChecks();
            DisplayNotifications();

            ShowMyAppointments = new NavigateCommand(_navigationStore, ViewType.MyAppointments, _patient);
            ShowSearchForDoctors = new NavigateCommand(_navigationStore, ViewType.SearchDoctors, _patient);
            ShowMyPrescriptions = new NavigateCommand(_navigationStore, ViewType.MyPrescriptions, _patient);
            ShowMyHealthRecord = new NavigateCommand(_navigationStore, ViewType.MyHealthRecord, _patient);
            ShowDoctorSurvey = new NavigateCommand(_navigationStore, ViewType.DoctorSurvey, _patient);
            ShowHealthCenterSurvey = new NavigateCommand(_navigationStore, ViewType.HealthCenterSurvey, _patient);
            LogOut = new LogOutCommand();
        }

        private void DisplayNotifications()
        {
            List<Notification> notifications = _notificationService.GetUnopened(_patient);
            if (notifications.Count == 0)
            {
                return;
            }
            MessageBox.Show("You have new notifications.");
            foreach (Notification notification in notifications)
            {
                MessageBox.Show(notification.Message);
            }
        }

        private void OnCurrentViewModelChanged()
        {
            CurrentViewLabel = CurrentViewModel.ToString();
            OnPropertyChanged(nameof(CurrentViewLabel));
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        private void StartNotificationChecks()
        {
            DispatcherTimer notificationDispatcherTimer = new DispatcherTimer();
            notificationDispatcherTimer.Tick += new EventHandler(NotificationChecker);
            notificationDispatcherTimer.Interval = new TimeSpan(0, 15, 0);
            notificationDispatcherTimer.Start();
        }

        private void NotificationChecker(object sender, EventArgs e)
        {
            ShowNotifications();
        }

        private void ShowNotifications()
        {
            List<string> notificationsToSend = _notificationService.GetNotifications(
                _notificationsFromPrescriptionsToSend, _patient);

            foreach (string notificationInfo in notificationsToSend)
            {
                _ = MessageBox.Show(notificationInfo, "My App", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
