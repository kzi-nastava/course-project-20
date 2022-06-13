using HealthCareCenter.Core;
using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Appointments.Services;
using HealthCareCenter.GUI.Patient.AppointmentCRUD.Commands;
using HealthCareCenter.GUI.Patient.SharedCommands;
using HealthCareCenter.GUI.Patient.SharedViewModels;
using System.Collections.Generic;
using System.Windows.Input;

namespace HealthCareCenter.GUI.Patient.AppointmentCRUD.ViewModels
{
    internal class MyAppointmentsViewModel : ViewModelBase
    {
        public Core.Patients.Patient Patient { get; }

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

        public MyAppointmentsViewModel(NavigationStore navigationStore, Core.Patients.Patient patient)
        {
            Patient = patient;

            _appointments = new List<AppointmentViewModel>();
            List<Appointment> unfinishedAppointments = AppointmentService.GetPatientUnfinishedAppointments(patient.HealthRecordID);
            foreach (Appointment appointment in unfinishedAppointments)
            {
                _appointments.Add(new AppointmentViewModel(appointment));
            }

            CreateAppointment = new ShowAppointmentFormCommand(this, navigationStore, false);
            ModifyAppointment = new ShowAppointmentFormCommand(this, navigationStore, true);
            CancelAppointment = new CancelAppointmentCommand(this, navigationStore);
            PriorityScheduling = new NavigateCommand(navigationStore, ViewType.PriorityScheduling, patient);
        }
    }
}
