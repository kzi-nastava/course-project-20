using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Appointments.Services;
using HealthCareCenter.GUI.Patient.Profile.ViewModels;
using HealthCareCenter.GUI.Patient.SharedCommands;
using HealthCareCenter.GUI.Patient.SharedViewModels;
using System.Collections.Generic;

namespace HealthCareCenter.GUI.Patient.Profile.Commands
{
    internal class SearchAppointmentsCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            List<Appointment> appointmentsByKeyword = AppointmentService.GetByAnamnesisKeyword(
                _viewModel.SearchKeyword, _viewModel.HealthRecord.ID);

            List<AppointmentViewModel> searchedAppointmentViewModels = new List<AppointmentViewModel>();
            foreach (Appointment appointment in appointmentsByKeyword)
            {
                searchedAppointmentViewModels.Add(new AppointmentViewModel(appointment));
            }

            _viewModel.Appointments = searchedAppointmentViewModels;
        }

        private readonly MyHealthRecordViewModel _viewModel;

        public SearchAppointmentsCommand(MyHealthRecordViewModel viewModel)
        {
            _viewModel = viewModel;
        }
    }
}
