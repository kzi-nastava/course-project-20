using HealthCareCenter.Model;
using HealthCareCenter.PatientGUI.Models;
using HealthCareCenter.PatientGUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;

namespace HealthCareCenter.PatientGUI.Commands
{
    internal class FindSchedulesCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            if (_viewModel.ChosenDate.Date.CompareTo(DateTime.Now.Date) <= 0)
            {
                _ = MessageBox.Show("Invalid date", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_viewModel.ChosenDoctor == null)
            {
                _ = MessageBox.Show("Doctor not chosen", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        private readonly AppointmentManipulationViewModel _viewModel;

        public FindSchedulesCommand(AppointmentManipulationViewModel viewModel)
        {
            _viewModel = viewModel;
        }
    }
}
