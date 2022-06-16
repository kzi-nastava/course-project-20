using HealthCareCenter.Core;
using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Appointments.Repository;
using HealthCareCenter.Core.Appointments.Services;
using HealthCareCenter.Core.HealthRecords;
using HealthCareCenter.Core.Patients.Services;
using HealthCareCenter.Core.Rooms.Repositories;
using HealthCareCenter.Core.Rooms.Services;
using HealthCareCenter.Core.Users;
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

        public MyAppointmentsViewModel(
            IAppointmentService appointmentService,
            Core.Patients.Patient patient,
            NavigationStore navigationStore)
        {
            Patient = patient;

            _appointments = new List<AppointmentViewModel>();
            List<Appointment> unfinishedAppointments = appointmentService.GetPatientUnfinishedAppointments(patient.HealthRecordID);
            foreach (Appointment appointment in unfinishedAppointments)
            {
                _appointments.Add(new AppointmentViewModel(appointment));
            }

            CreateAppointment = new ShowAppointmentFormCommand(this, navigationStore, false);
            ModifyAppointment = new ShowAppointmentFormCommand(this, navigationStore, true);
            CancelAppointment = new CancelAppointmentCommand(
                this, 
                navigationStore,
                new AppointmentService(
                        new AppointmentRepository(),
                        new AppointmentChangeRequestRepository(),
                        new AppointmentChangeRequestService(
                            new AppointmentRepository(),
                            new AppointmentChangeRequestRepository(),
                            new HospitalRoomService(
                                new AppointmentRepository(),
                                new HospitalRoomForRenovationService(
                                    new HospitalRoomForRenovationRepository()),
                                new HospitalRoomRepository()),
                            new UserRepository()),
                        new PatientService(
                            new AppointmentRepository(),
                            new AppointmentChangeRequestRepository(),
                            new HealthRecordRepository(),
                            new HealthRecordService(
                                new HealthRecordRepository()),
                            new PatientEditService(
                                new HealthRecordRepository(),
                                new UserRepository()),
                            new UserRepository()),
                        new HospitalRoomService(
                            new AppointmentRepository(),
                            new HospitalRoomForRenovationService(
                                new HospitalRoomForRenovationRepository()),
                            new HospitalRoomRepository()),
                        new HospitalRoomRepository()));
            PriorityScheduling = new NavigateCommand(navigationStore, ViewType.PriorityScheduling, patient);
        }
    }
}
