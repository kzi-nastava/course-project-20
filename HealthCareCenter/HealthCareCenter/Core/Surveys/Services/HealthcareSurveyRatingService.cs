using HealthCareCenter.Core.Surveys.Models;
using HealthCareCenter.Core.Surveys.Repositories;

namespace HealthCareCenter.Core.Surveys.Services
{
    class HealthcareSurveyRatingService : IHealthcareSurveyRatingService
    {
        private readonly BaseHealthcareSurveyRatingRepository _healthcareRatingRepository;

        public HealthcareSurveyRatingService(BaseHealthcareSurveyRatingRepository healthcareRatingRepository)
        {
            _healthcareRatingRepository = healthcareRatingRepository;
        }

        public double GetAverageRating()
        {
            double average = 0.0;
            double count = 0.0;
            foreach (SurveyRating rating in _healthcareRatingRepository.Ratings)
            {
                average += rating.Rating;
                ++count;
            }

            return count == 0.0 ? 0.0 : average / count;
        }

        public bool HasPatientAlreadyReviewed(int patientID)
        {
            foreach (SurveyRating rating in _healthcareRatingRepository.Ratings)
            {
                if (patientID == rating.PatientID)
                {
                    return true;
                }
            }

            return false;
        }

        public void OverwriteExistingReview(SurveyRating surveyRating)
        {
            foreach (SurveyRating rating in _healthcareRatingRepository.Ratings)
            {
                if (rating.PatientID == surveyRating.PatientID)
                {
                    rating.Comment = surveyRating.Comment;
                    rating.Rating = surveyRating.Rating;
                    _healthcareRatingRepository.Save();
                    break;
                }
            }
        }

        public void AddRating(SurveyRating rating)
        {
            _healthcareRatingRepository.Ratings.Add(rating);
            _healthcareRatingRepository.Save();
        }
    }
}
