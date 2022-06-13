using HealthCareCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Controller
{
    internal class HealthcareSurveysOverviewController
    {
        public List<SurveyRating> GetSurveys()
        {
            return HealthcareSurveyRatingRepository.Ratings;
        }
    }
}