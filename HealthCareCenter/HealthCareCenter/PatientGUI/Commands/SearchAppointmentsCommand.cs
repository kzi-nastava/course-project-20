using HealthCareCenter.Model;
using HealthCareCenter.PatientGUI.Models;
using HealthCareCenter.PatientGUI.ViewModels;
using HealthCareCenter.Service;
using System.Collections.Generic;

namespace HealthCareCenter.PatientGUI.Commands
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
