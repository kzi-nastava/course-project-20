using HealthCareCenter.PatientGUI.Models;
using HealthCareCenter.PatientGUI.Stores;
using HealthCareCenter.PatientGUI.ViewModels;
using HealthCareCenter.Service;
using System;
using System.Windows;

namespace HealthCareCenter.PatientGUI.Commands
{
    internal class ModifyAppointmentCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            if (_viewModel.ChosenSchedule == null)
            {
                _ = MessageBox.Show("No schedule selected", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DateTime scheduleDate = Convert.ToDateTime(
                _viewModel.ChosenSchedule.AvailableDate + " " + _viewModel.ChosenSchedule.AvailableTerm);

            int hospitalRoomID = HospitalRoomService.GetAvailableRoomID(scheduleDate, Enums.RoomType.Checkup);
            if (hospitalRoomID == -1)
            {
                MessageBox.Show("No available room in that schedule", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            PatientFunctionality patFunc = PatientFunctionality.GetInstance();
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure?", "Schedule appointment?", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                DateTime oldScheduleDate = Convert.ToDateTime(_viewModel.ChosenAppointment.AppointmentDate);
                if (patFunc.ShouldSendToSecretary(oldScheduleDate))
                {
                    _ = MessageBox.Show("Since there are less than 2 days until this appointment starts, a request will be sent to the secretary",
                        "My App", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                patFunc.ModifyAppointment(
                    scheduleDate, oldScheduleDate, Convert.ToInt32(_viewModel.ChosenAppointment.AppointmentID),
                    Convert.ToInt32(_viewModel.ChosenDoctor.DoctorID), _viewModel.Patient.ID, hospitalRoomID);
                _navigationStore.CurrentViewModel = new MyAppointmentsViewModel(_navigationStore, _viewModel.Patient);
            }
        }

        private readonly NavigationStore _navigationStore;
        private readonly ModifyAppointmentViewModel _viewModel;

        public ModifyAppointmentCommand(NavigationStore navigationStore, ModifyAppointmentViewModel viewModel)
        {
            _navigationStore = navigationStore;
            _viewModel = viewModel;
        }
    }
}
