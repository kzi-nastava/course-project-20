using HealthCareCenter.Model;
using HealthCareCenter.PatientGUI.Stores;
using HealthCareCenter.PatientGUI.ViewModels;
using HealthCareCenter.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace HealthCareCenter.PatientGUI.Commands
{
    class SubmitDoctorReviewCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            if (_viewModel.ChosenAppointment == null)
            {
                _ = MessageBox.Show("Appointment not chosen", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            double serviceQualityRating = GetTickedGradeServiceQuality();
            double wouldRecommendRating = GetTickedGradeWouldRecommend();

            string comment = _viewModel.CommentOnDoctor;
            double rating = (serviceQualityRating + wouldRecommendRating) / 2.0;
            DoctorSurveyRating surveyRating = new DoctorSurveyRating(
                _viewModel.ChosenAppointment.DoctorID, _viewModel.Patient.ID, rating, comment);

            string confirmationMessage = "Submit review?";
            if (DoctorSurveyRatingService.HasPatientAlreadyReviewed(_viewModel.Patient.ID, _viewModel.ChosenAppointment.DoctorID))
            {
                _ = MessageBox.Show("You already reviewed this doctor", "My App", MessageBoxButton.OK, MessageBoxImage.Information);
                confirmationMessage = "Do you want to overwrite your previous review?";
            }

            MessageBoxResult messageBoxResult = MessageBox.Show(confirmationMessage, "Schedule appointment?", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                DoctorSurveyRatingRepository.Ratings.Add(surveyRating);
                DoctorSurveyRatingRepository.Save();
            }


        }

        private readonly DoctorSurveyViewModel _viewModel;
        private readonly NavigationStore _navigationStore;

        public SubmitDoctorReviewCommand(DoctorSurveyViewModel viewModel, NavigationStore navigatonStore)
        {
            _viewModel = viewModel;
            _navigationStore = navigatonStore;
        }

        private double GetTickedGradeServiceQuality()
        {
            if (_viewModel.ServiceQualityTicked1)
            {
                return 1;
            }
            else if (_viewModel.ServiceQualityTicked2)
            {
                return 2;
            }
            else if (_viewModel.ServiceQualityTicked3)
            {
                return 3;
            }
            else if (_viewModel.ServiceQualityTicked4)
            {
                return 4;
            }
            else
            {
                return 5;
            }
        }

        private double GetTickedGradeWouldRecommend()
        {
            if (_viewModel.WouldRecommendTicked1)
            {
                return 1;
            }
            else if (_viewModel.WouldRecommendTicked2)
            {
                return 2;
            }
            else if (_viewModel.WouldRecommendTicked3)
            {
                return 3;
            }
            else if (_viewModel.WouldRecommendTicked4)
            {
                return 4;
            }
            else
            {
                return 5;
            }
        }
    }
}
