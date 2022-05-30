using HealthCareCenter.Model;
using HealthCareCenter.PatientGUI.Commands;
using HealthCareCenter.PatientGUI.Models;
using HealthCareCenter.PatientGUI.Stores;
using HealthCareCenter.Service;
using System.Collections.Generic;
using System.Windows.Input;

namespace HealthCareCenter.PatientGUI.ViewModels
{
    internal class MyAppointmentsViewModel : ViewModelBase
    {
        public Patient Patient { get; }

        private readonly List<AppointmentViewModel> _appointments;
        public List<AppointmentViewModel> Appointments => _appointments;

        private AppointmentViewModel _chosenAppointment;
        public AppointmentViewModel ChosenAppointment
        {
            get => _chosenAppointment;
            set
            {
                _chosenAppointment = value;
                OnPropertyChanged(nameof(ChosenAppointment));
            }
        }

        public ICommand CreateAppointment { get; }
        public ICommand ModifyAppointment { get; }
        public ICommand CancelAppointment { get; }
        public ICommand PriorityScheduling { get; }

        public MyAppointmentsViewModel(NavigationStore navigationStore, Patient patient)
        {
            Patient = patient;

            _appointments = new List<AppointmentViewModel>();
            List<Appointment> unfinishedAppointments = AppointmentService.GetPatientUnfinishedAppointments(patient.HealthRecordID);
            foreach (Appointment appointment in unfinishedAppointments)
            {
                _appointments.Add(new AppointmentViewModel(appointment));
            }

            CreateAppointment = new NavigateCommand(navigationStore, ViewType.CreateAppointment, patient);
            ModifyAppointment = new ShowModifyAppointmentCommand(this, navigationStore);
            CancelAppointment = new CancelAppointmentCommand(this, navigationStore);
            PriorityScheduling = new NavigateCommand(navigationStore, ViewType.PriorityScheduling, patient);
        }
    }
}
