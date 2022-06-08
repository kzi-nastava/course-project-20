using HealthCareCenter.Model;
using HealthCareCenter.PatientGUI.ViewModels;
using HealthCareCenter.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace HealthCareCenter.PatientGUI.Commands
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

            Doctor doctor = DoctorService.Get(_viewModel.ChosenAppointment.DoctorID);
            _viewModel.DoctorFullName = doctor.FirstName + " " + doctor.LastName;
        }

        DoctorSurveyViewModel _viewModel;

        public ChooseDoctorFromAppointmentCommand(DoctorSurveyViewModel viewModel)
        {
            _viewModel = viewModel;
        }
    }
}
