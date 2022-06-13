using HealthCareCenter.Core.Users.Services;
using HealthCareCenter.GUI.Patient.SharedCommands;
using HealthCareCenter.GUI.Patient.SharedViewModels;
using System.Collections.Generic;
using System.Windows;

namespace HealthCareCenter.GUI.Patient.DoctorSearch
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

            List<Core.Users.Models.Doctor> doctorsByKeyword = DoctorService.SearchByKeyword(
                _viewModel.SearchKeyword.Trim().ToLower(), _viewModel.ChosenSearchCriteria);

            List<DoctorViewModel> searchedDoctors = new List<DoctorViewModel>();
            foreach (Core.Users.Models.Doctor doctor in doctorsByKeyword)
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
