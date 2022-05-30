using HealthCareCenter.Model;
using HealthCareCenter.PatientGUI.Models;
using HealthCareCenter.PatientGUI.ViewModels;
using HealthCareCenter.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace HealthCareCenter.PatientGUI.Commands
{
    class SortDoctorsCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            if (_viewModel.Doctors.Count == 0)
            {
                MessageBox.Show("Nothing to sort", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            PatientFunctionality patFunc = PatientFunctionality.GetInstance();

            List<Doctor> searchedDoctors = new List<Doctor>();
            foreach (DoctorViewModel doctorViewModel in _viewModel.Doctors)
            {
                searchedDoctors.Add(UserService.GetDoctor(Convert.ToInt32(doctorViewModel.DoctorID)));
            }

            List<Doctor> sortedDoctors = patFunc.GetSortedDoctorsByCriteria(
                searchedDoctors, _viewModel.ChosenSortCriteria, _viewModel.ChosenSearchCriteria);

            List<DoctorViewModel> sortedDoctorViewModels = new List<DoctorViewModel>();
            foreach (Doctor doctor in sortedDoctors)
            {
                sortedDoctorViewModels.Add(new DoctorViewModel(doctor));
            }

            _viewModel.Doctors = sortedDoctorViewModels;
        }

        private SearchDoctorsViewModel _viewModel;

        public SortDoctorsCommand(SearchDoctorsViewModel viewModel)
        {
            _viewModel = viewModel;
        }
    }
}
