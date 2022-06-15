using HealthCareCenter.Core.Surveys.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Surveys.Services
{
    public interface IHealthcareSurveyRatingService
    {
        double GetAverageRating();
        bool HasPatientAlreadyReviewed(int patientID);
        void OverwriteExistingReview(SurveyRating surveyRating);
        void AddRating(SurveyRating rating);
    }
}
