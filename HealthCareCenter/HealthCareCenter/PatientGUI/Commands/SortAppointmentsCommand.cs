using HealthCareCenter.Model;
using HealthCareCenter.PatientGUI.Models;
using HealthCareCenter.PatientGUI.ViewModels;
using HealthCareCenter.Service;
using System;
using System.Collections.Generic;

namespace HealthCareCenter.PatientGUI.Commands
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

            PatientFunctionality patFunc = PatientFunctionality.GetInstance();
            appointments = patFunc.SortAppointments(appointments, _viewModel.ChosenSortCriteria);

            List<AppointmentViewModel> sortedAppointmentViewModels = new List<AppointmentViewModel>();
            foreach (Appointment appointment in appointments)
            {
                sortedAppointmentViewModels.Add(new AppointmentViewModel(appointment));
            }

            _viewModel.Appointments = sortedAppointmentViewModels;
        }

        private readonly MyHealthRecordViewModel _viewModel;

        public SortAppointmentsCommand(MyHealthRecordViewModel viewModel)
        {
            _viewModel = viewModel;
        }
    }
}
