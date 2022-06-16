using HealthCareCenter.Core.Surveys.Repositories;
using HealthCareCenter.Core.Surveys.Services;
using HealthCareCenter.Core.Users.Services;
using HealthCareCenter.GUI.Patient.SharedCommands;
using HealthCareCenter.GUI.Patient.SharedViewModels;
using System.Collections.Generic;
using System.Windows;

namespace HealthCareCenter.GUI.Patient.DoctorSearch
{
    internal class SortDoctorsCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            if (_viewModel.Doctors.Count == 0)
            {
                _ = MessageBox.Show("Nothing to sort", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            List<Core.Users.Models.Doctor> searchedDoctors = new List<Core.Users.Models.Doctor>();
            foreach (DoctorViewModel doctorViewModel in _viewModel.Doctors)
            {
                searchedDoctors.Add(DoctorService.Get(doctorViewModel.DoctorID));
            }

            List<Core.Users.Models.Doctor> sortedDoctors = DoctorService.GetSortedByCriteria(
                searchedDoctors, _viewModel.ChosenSortCriteria, _viewModel.ChosenSearchCriteria);

            List<DoctorViewModel> sortedDoctorViewModels = new List<DoctorViewModel>();
            foreach (Core.Users.Models.Doctor doctor in sortedDoctors)
            {
                sortedDoctorViewModels.Add(new DoctorViewModel(doctor, new DoctorSurveyRatingService(new DoctorSurveyRatingRepository())));
            }

            _viewModel.Doctors = sortedDoctorViewModels;
        }

        private readonly SearchDoctorsViewModel _viewModel;

        public SortDoctorsCommand(SearchDoctorsViewModel viewModel)
        {
            _viewModel = viewModel;
        }
    }
}