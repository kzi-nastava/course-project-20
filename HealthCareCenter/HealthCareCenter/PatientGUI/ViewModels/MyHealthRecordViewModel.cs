using HealthCareCenter.PatientGUI.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace HealthCareCenter.PatientGUI.ViewModels
{
    class MyHealthRecordViewModel : ViewModelBase
    {
        private readonly ObservableCollection<AppointmentViewModel> _appointments;
        public IEnumerable<AppointmentViewModel> Appointments => _appointments;

        public ICommand SortAppointments { get; }
        public ICommand SearchAppointments { get; }
        public ICommand ShowAnamnesis { get; }

        public MyHealthRecordViewModel(NavigationStore navigationStore)
        {
            _appointments = new ObservableCollection<AppointmentViewModel>();
            List<Model.Appointment> allAppointments = Model.AppointmentRepository.Load();
            foreach (Model.Appointment appointment in allAppointments)
            {
                _appointments.Add(new AppointmentViewModel(appointment));
            }


        }
    }
}
