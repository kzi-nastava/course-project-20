using HealthCareCenter.Model;
using HealthCareCenter.PatientGUI.Models;
using HealthCareCenter.PatientGUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace HealthCareCenter.PatientGUI.Commands
{
    class FindSchedulesCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            if (_viewModel.ChosenDate.Date.CompareTo(DateTime.Now.Date) <= 0)
            {
                MessageBox.Show("Invalid date", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_viewModel.ChosenDoctor == null)
            {
                MessageBox.Show("Doctor not chosen", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            PatientFunctionality patFunc = PatientFunctionality.GetInstance();
            List<AppointmentTerm> allPossibleTerms = patFunc.GetAllPossibleTermsForCreateAppointment(
                Convert.ToInt32(_viewModel.ChosenDoctor.DoctorID), _viewModel.ChosenDate);

            List<AppointmentScheduleViewModel> availableTerms = new List<AppointmentScheduleViewModel>();
            foreach (AppointmentTerm term in allPossibleTerms)
            {
                availableTerms.Add(new AppointmentScheduleViewModel(_viewModel.ChosenDate.ToString("d"), term.ToString()));
            }

            _viewModel.AvailableSchedules = availableTerms;
        }

        CreateAppointmentViewModel _viewModel;

        public FindSchedulesCommand(CreateAppointmentViewModel viewModel)
        {
            _viewModel = viewModel;
        }
    }
}
