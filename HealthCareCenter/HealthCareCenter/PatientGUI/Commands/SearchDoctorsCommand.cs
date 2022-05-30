using HealthCareCenter.Model;
using HealthCareCenter.PatientGUI.Models;
using HealthCareCenter.PatientGUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace HealthCareCenter.PatientGUI.Commands
{
    class SearchDoctorsCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            if (string.IsNullOrEmpty(_viewModel.SearchKeyword))
            {
                MessageBox.Show("Keyword not entered", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        SearchDoctorsViewModel _viewModel;

        public SearchDoctorsCommand(SearchDoctorsViewModel viewModel)
        {
            _viewModel = viewModel;
        }
    }
}
