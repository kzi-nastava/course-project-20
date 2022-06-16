using HealthCareCenter.Core.Surveys.Repositories;
using HealthCareCenter.Core.Surveys.Services;
using HealthCareCenter.Core.Users;
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
                searchedDoctors.Add(_doctorService.Get(doctorViewModel.DoctorID));
            }

            List<Core.Users.Models.Doctor> sortedDoctors = _doctorService.GetSortedByCriteria(
                searchedDoctors, _viewModel.ChosenSortCriteria, _viewModel.ChosenSearchCriteria);

            List<DoctorViewModel> sortedDoctorViewModels = new List<DoctorViewModel>();
            foreach (Core.Users.Models.Doctor doctor in sortedDoctors)
            {
                sortedDoctorViewModels.Add(new DoctorViewModel(doctor, new DoctorSurveyRatingService(new DoctorSurveyRatingRepository(), new UserRepository())));
            }

            _viewModel.Doctors = sortedDoctorViewModels;
        }

        private readonly SearchDoctorsViewModel _viewModel;
        private readonly IDoctorService _doctorService;

        public SortDoctorsCommand(
            SearchDoctorsViewModel viewModel,
            IDoctorService doctorService)
        {
            _viewModel = viewModel;
            _doctorService = doctorService;
        }
    }
}