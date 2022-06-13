using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Appointments.Services;
using HealthCareCenter.GUI.Patient.Profile.ViewModels;
using HealthCareCenter.GUI.Patient.SharedCommands;
using HealthCareCenter.GUI.Patient.SharedViewModels;
using System;
using System.Collections.Generic;

namespace HealthCareCenter.GUI.Patient.Profile.Commands
{
    internal class SortAppointmentsCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            List<Appointment> appointments = new List<Appointment>();
            foreach (AppointmentViewModel appointmentViewModel in _viewModel.Appointments)
            {
                appointments.Add(AppointmentService.Get(appointmentViewModel.AppointmentID));
            }

            appointments = AppointmentService.Sort(appointments, _viewModel.ChosenSortCriteria);

            List<AppointmentViewModel> sortedAppointmentViewModels = new List<AppointmentViewModel>();
            foreach (Appointment appointment in appointments)
            {
                sortedAppointmentViewModels.Add(new AppointmentViewModel(appointment));
            }

            _viewModel.Appointments = sortedAppointmentViewModels;
            _viewModel.AnamnesisInfo = "";
        }

        private readonly MyHealthRecordViewModel _viewModel;

        public SortAppointmentsCommand(MyHealthRecordViewModel viewModel)
        {
            _viewModel = viewModel;
        }
    }
}
