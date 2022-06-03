using HealthCareCenter.PatientGUI.Models;
using HealthCareCenter.PatientGUI.Stores;
using HealthCareCenter.PatientGUI.ViewModels;
using HealthCareCenter.Service;
using System;
using System.Windows;

namespace HealthCareCenter.PatientGUI.Commands
{
    internal class SubmitAppointmentCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            DateTime scheduleDate = Convert.ToDateTime(
                _viewModel.ChosenDate.ToString("d") + " " + _viewModel.ChosenTerm.ToString());
            int hospitalRoomID = HospitalRoomService.GetAvailableRoomID(scheduleDate, Enums.RoomType.Checkup);

            if (!IsValid(scheduleDate, hospitalRoomID))
            {
                return;
            }

            if (_viewModel.ChosenAppointment == null)
            {
                ScheduleAppointment(scheduleDate, hospitalRoomID);
            }
            else
            {
                ModifyAppointment(scheduleDate, hospitalRoomID);
            }

        }

        private readonly NavigationStore _navigationStore;
        private readonly AppointmentFormViewModel _viewModel;

        public SubmitAppointmentCommand(NavigationStore navigationStore, AppointmentFormViewModel viewModel)
        {
            _navigationStore = navigationStore;
            _viewModel = viewModel;
        }

        private bool IsValid(DateTime scheduleDate, int hospitalRoomID)
        {
            if (_viewModel.ChosenDate.Date.CompareTo(DateTime.Now.Date) <= 0)
            {
                _ = MessageBox.Show("Invalid date", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (_viewModel.ChosenDoctor == null)
            {
                _ = MessageBox.Show("Doctor not chosen", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (_viewModel.ChosenTerm == null)
            {
                _ = MessageBox.Show("No schedule selected", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            PatientFunctionality patFunc = PatientFunctionality.GetInstance();
            if (!patFunc.IsAvailable(scheduleDate, _viewModel.ChosenDoctor.DoctorID))
            {
                _ = MessageBox.Show("That schedule is unavailable", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (hospitalRoomID == -1)
            {
                _ = MessageBox.Show("No available room in that schedule", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void ScheduleAppointment(DateTime scheduleDate, int hospitalRoomID)
        {
            PatientFunctionality patFunc = PatientFunctionality.GetInstance();
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure?", "Schedule appointment?", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                patFunc.ScheduleAppointment(
                    scheduleDate, _viewModel.ChosenDoctor.DoctorID, _viewModel.Patient.HealthRecordID, hospitalRoomID);
                _navigationStore.CurrentViewModel = new MyAppointmentsViewModel(_navigationStore, _viewModel.Patient);
            }
        }

        private void ModifyAppointment(DateTime scheduleDate, int hospitalRoomID)
        {
            PatientFunctionality patFunc = PatientFunctionality.GetInstance();
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure?", "Schedule appointment?", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (patFunc.ShouldSendToSecretary(_viewModel.ChosenAppointment.AppointmentDate))
                {
                    _ = MessageBox.Show("Since there are less than 2 days until this appointment starts, a request will be sent to the secretary",
                        "My App", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                patFunc.ModifyAppointment(
                    scheduleDate, _viewModel.ChosenAppointment.AppointmentDate, _viewModel.ChosenAppointment.AppointmentID,
                    _viewModel.ChosenDoctor.DoctorID, _viewModel.Patient.ID, hospitalRoomID);
                _navigationStore.CurrentViewModel = new MyAppointmentsViewModel(_navigationStore, _viewModel.Patient);
            }
        }
    }
}
