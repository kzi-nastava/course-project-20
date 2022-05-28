using HealthCareCenter.PatientGUI.Commands;
using HealthCareCenter.PatientGUI.Models;
using HealthCareCenter.PatientGUI.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace HealthCareCenter.PatientGUI.ViewModels
{
    class MyAppointmentsViewModel : ViewModelBase
    {
        private readonly ObservableCollection<AppointmentViewModel> _appointments;
        public IEnumerable<AppointmentViewModel> Appointments => _appointments;

        public ICommand CreateAppointment { get; }
        public ICommand ModifyAppointment { get; }
        public ICommand CancelAppointment { get; }
        public ICommand PriorityScheduling { get; }

        public MyAppointmentsViewModel(NavigationStore navigationStore)
        {
            _appointments = new ObservableCollection<AppointmentViewModel>();
            List<Model.Appointment> allAppointments = Model.AppointmentRepository.Load();
            foreach (Model.Appointment appointment in allAppointments)
            {
                _appointments.Add(new AppointmentViewModel(appointment));
            }

            CreateAppointment = new NavigateCommand(navigationStore, ViewType.CreateAppointment);
            ModifyAppointment = new NavigateCommand(navigationStore, ViewType.ModifyAppointment);
            PriorityScheduling = new NavigateCommand(navigationStore, ViewType.PriorityScheduling);
        }
    }
}
