using HealthCareCenter.Core.Surveys.Models;
using HealthCareCenter.Core.Surveys.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Surveys.Controllers
{
    internal class HealthcareSurveysOverviewController
    {
        public List<SurveyRating> GetSurveys()
        {
            return HealthcareSurveyRatingRepository.Ratings;
        }
    }
}