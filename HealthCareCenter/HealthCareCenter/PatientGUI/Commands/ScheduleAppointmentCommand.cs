using HealthCareCenter.PatientGUI.Models;
using HealthCareCenter.PatientGUI.Stores;
using HealthCareCenter.PatientGUI.ViewModels;
using HealthCareCenter.Service;
using System;
using System.Windows;

namespace HealthCareCenter.PatientGUI.Commands
{
    internal class ScheduleAppointmentCommand : CommandBase
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
                _ = MessageBox.Show("No available room in that schedule", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            PatientFunctionality patFunc = PatientFunctionality.GetInstance();
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure?", "Schedule appointment?", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                patFunc.ScheduleAppointment(
                    scheduleDate, _viewModel.ChosenDoctor.DoctorID, _viewModel.Patient.HealthRecordID, hospitalRoomID);
                _navigationStore.CurrentViewModel = new MyAppointmentsViewModel(_navigationStore, _viewModel.Patient);
            }
        }

        private readonly NavigationStore _navigationStore;
        private readonly CreateAppointmentViewModel _viewModel;

        public ScheduleAppointmentCommand(NavigationStore navigationStore, CreateAppointmentViewModel viewModel)
        {
            _navigationStore = navigationStore;
            _viewModel = viewModel;
        }
    }
}
