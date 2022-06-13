using HealthCareCenter.Core.Users.Services;
using HealthCareCenter.GUI.Patient.SharedCommands;
using HealthCareCenter.GUI.Patient.Survey.ViewModels;
using System.Windows;

namespace HealthCareCenter.GUI.Patient.Survey.Commands
{
    class ChooseDoctorFromAppointmentCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            if (_viewModel.ChosenAppointment == null)
            {
                _ = MessageBox.Show("Appointment not chosen", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Core.Users.Models.Doctor doctor = DoctorService.Get(_viewModel.ChosenAppointment.DoctorID);
            _viewModel.DoctorFullName = doctor.FirstName + " " + doctor.LastName;
        }

        DoctorSurveyViewModel _viewModel;

        public ChooseDoctorFromAppointmentCommand(DoctorSurveyViewModel viewModel)
        {
            _viewModel = viewModel;
        }
    }
}
