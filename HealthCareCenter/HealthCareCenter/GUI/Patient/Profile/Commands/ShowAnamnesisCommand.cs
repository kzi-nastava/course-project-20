using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Appointments.Services;
using HealthCareCenter.GUI.Patient.Profile.ViewModels;
using HealthCareCenter.GUI.Patient.SharedCommands;
using System;
using System.Windows;

namespace HealthCareCenter.GUI.Patient.Profile.Commands
{
    internal class ShowAnamnesisCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            if (_viewModel.ChosenAppointment == null)
            {
                MessageBox.Show("Appointment not chosen", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Appointment appointment = _appointmentService.Get(_viewModel.ChosenAppointment.AppointmentID);
            if (appointment.PatientAnamnesis != null)
            {
                _viewModel.AnamnesisInfo = appointment.PatientAnamnesis.Comment;
            }
        }

        private readonly MyHealthRecordViewModel _viewModel;
        private readonly IAppointmentService _appointmentService;

        public ShowAnamnesisCommand(
            MyHealthRecordViewModel viewModel,
            IAppointmentService appointmentService)
        {
            _viewModel = viewModel;
            _appointmentService = appointmentService;
        }
    }
}
