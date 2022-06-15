using HealthCareCenter.Core.Surveys.Models;
using HealthCareCenter.Core.Surveys.Repositories;
using HealthCareCenter.Core.Surveys.Services;
using HealthCareCenter.GUI.Patient.SharedCommands;
using HealthCareCenter.GUI.Patient.Survey.ViewModels;
using System.Windows;

namespace HealthCareCenter.GUI.Patient.Survey.Commands
{
    class SubmitHealthcareCenterReviewCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            double serviceQualityRating = GetTickedGradeServiceQuality();
            double hygieneQualityRating = GetTickedGradeHygieneQuality();
            double satisfiedRating = GetTickedGradeSatisfied();
            double wouldRecommendRating = GetTickedGradeWouldRecommend();

            double rating = (serviceQualityRating + hygieneQualityRating + satisfiedRating + wouldRecommendRating) / 4.0;
            string comment = _viewModel.CommentOnHealthcareCenter;

            SurveyRating surveyRating = new SurveyRating(_viewModel.Patient.ID, comment, rating);

            string confirmationMessage = "Submit review?";
            bool isOverwrite = false;
            if (_healthcareRatingService.HasPatientAlreadyReviewed(_viewModel.Patient.ID))
            {
                _ = MessageBox.Show("You already have a review", "My App", MessageBoxButton.OK, MessageBoxImage.Information);
                confirmationMessage = "Do you want to overwrite your previous review?";
                isOverwrite = true;
            }

            MessageBoxResult messageBoxResult = MessageBox.Show(confirmationMessage, "", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (isOverwrite)
                {
                    _healthcareRatingService.OverwriteExistingReview(surveyRating);
                }
                else
                {
                    _healthcareRatingService.AddRating(surveyRating);
                }

                _viewModel.CommentOnHealthcareCenter = "";
            }
        }

        private readonly HealthCenterSurveyViewModel _viewModel;
        private readonly BaseHealthcareSurveyRatingRepository _healthcareRatingRepository;
        private readonly IHealthcareSurveyRatingService _healthcareRatingService;

        public SubmitHealthcareCenterReviewCommand(
            HealthCenterSurveyViewModel viewModel,
            BaseHealthcareSurveyRatingRepository healthcareRatingRepository,
            IHealthcareSurveyRatingService healthcareRatingService)
        {
            _viewModel = viewModel;
            _healthcareRatingRepository = healthcareRatingRepository;
            _healthcareRatingService = healthcareRatingService;
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

        private double GetTickedGradeHygieneQuality()
        {
            if (_viewModel.HygieneQualityTicked1)
            {
                return 1;
            }
            else if (_viewModel.HygieneQualityTicked2)
            {
                return 2;
            }
            else if (_viewModel.HygieneQualityTicked3)
            {
                return 3;
            }
            else if (_viewModel.HygieneQualityTicked4)
            {
                return 4;
            }
            else
            {
                return 5;
            }
        }

        private double GetTickedGradeSatisfied()
        {
            if (_viewModel.SatisfiedTicked1)
            {
                return 1;
            }
            else if (_viewModel.SatisfiedTicked2)
            {
                return 2;
            }
            else if (_viewModel.SatisfiedTicked3)
            {
                return 3;
            }
            else if (_viewModel.SatisfiedTicked4)
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
