using HealthCareCenter.Core;
using HealthCareCenter.Core.Appointments.Repository;
using HealthCareCenter.Core.Appointments.Services;
using HealthCareCenter.Core.HealthRecords;
using HealthCareCenter.Core.Patients.Services;
using HealthCareCenter.GUI.Patient.AppointmentCRUD.ViewModels;
using HealthCareCenter.GUI.Patient.SharedCommands;
using HealthCareCenter.GUI.Patient.SharedViewModels;
using System.Windows;

namespace HealthCareCenter.GUI.Patient.AppointmentCRUD.Commands
{
    internal class CancelAppointmentCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            if (_viewModel.ChosenAppointment == null)
            {
                _ = MessageBox.Show("Appointment not chosen", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure?", "Schedule appointment?", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (_appointmentService.ShouldSendToSecretary(_viewModel.ChosenAppointment.AppointmentDate))
                {
                    _ = MessageBox.Show("Since there are less than 2 days until this appointment starts, a request will be sent to the secretary",
                        "My App", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                bool passed = _appointmentService.Cancel(
                    _viewModel.ChosenAppointment.AppointmentID, _viewModel.Patient.ID, _viewModel.ChosenAppointment.AppointmentDate);

                if (!passed)
                {
                    _ = MessageBox.Show("Trolling limit reached! This account will be blocked", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
                    _viewModel.Patient.IsBlocked = true;
                    _viewModel.Patient.BlockedBy = Blocker.System;

                    LoginWindow win = new LoginWindow();
                    win.Show();
                    Application.Current.Windows[0].Close();

                    return;
                }

                _navigationStore.CurrentViewModel = new MyAppointmentsViewModel(
                    new AppointmentService(
                        new AppointmentRepository(),
                        new AppointmentChangeRequestRepository(),
                        new AppointmentChangeRequestService(
                            new AppointmentRepository(),
                            new AppointmentChangeRequestRepository()),
                        new PatientService(
                            new AppointmentRepository(),
                            new AppointmentChangeRequestRepository(),
                            new HealthRecordRepository(),
                            new HealthRecordService(
                                new HealthRecordRepository()),
                            new PatientEditService(
                                new HealthRecordRepository()))),
                    _viewModel.Patient,
                    _navigationStore);
            }
        }

        private readonly MyAppointmentsViewModel _viewModel;
        private readonly NavigationStore _navigationStore;
        private readonly IAppointmentService _appointmentService;

        public CancelAppointmentCommand(
            MyAppointmentsViewModel viewModel,
            NavigationStore navigationStore,
            IAppointmentService appointmentService)
        {
            _viewModel = viewModel;
            _navigationStore = navigationStore;
            _appointmentService = appointmentService;
        }
    }
}
