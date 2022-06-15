using HealthCareCenter.Core.Surveys.Models;
using HealthCareCenter.Core.Surveys.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Surveys.Controllers
{
    internal class HealthcareSurveysOverviewController
    {
        private readonly BaseHealthcareSurveyRatingRepository _healthcareRatingRepository;

        public HealthcareSurveysOverviewController(BaseHealthcareSurveyRatingRepository healthcareRatingRepository)
        {
            _healthcareRatingRepository = healthcareRatingRepository;
        }

        public List<SurveyRating> GetSurveys()
        {
            return _healthcareRatingRepository.Ratings;
        }
    }
}