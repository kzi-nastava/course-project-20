using HealthCareCenter.Model;
using HealthCareCenter.PatientGUI.Models;
using HealthCareCenter.PatientGUI.ViewModels;
using System.Collections.Generic;
using System.Windows;

namespace HealthCareCenter.PatientGUI.Commands
{
    internal class SearchDoctorsCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            if (string.IsNullOrEmpty(_viewModel.SearchKeyword))
            {
                _ = MessageBox.Show("Keyword not entered", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            PatientFunctionality patFunc = PatientFunctionality.GetInstance();
            List<Doctor> doctorsByKeyword = patFunc.SearchDoctorByKeyword(
                _viewModel.SearchKeyword.Trim().ToLower(), _viewModel.ChosenSearchCriteria);

            List<DoctorViewModel> searchedDoctors = new List<DoctorViewModel>();
            foreach (Doctor doctor in doctorsByKeyword)
            {
                searchedDoctors.Add(new DoctorViewModel(doctor));
            }

            _viewModel.Doctors = searchedDoctors;

        }

        private readonly SearchDoctorsViewModel _viewModel;

        public SearchDoctorsCommand(SearchDoctorsViewModel viewModel)
        {
            _viewModel = viewModel;
        }
    }
}
