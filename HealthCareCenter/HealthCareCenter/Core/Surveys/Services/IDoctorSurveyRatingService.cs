using HealthCareCenter.Core.Surveys.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Surveys.Services
{
    public interface IDoctorSurveyRatingService
    {
        double GetAverageRating(int doctorID);

        Dictionary<int, double> GetAllRatings();

        bool HasPatientAlreadyReviewed(int patientID, int doctorID);

        void OverwriteExistingReview(DoctorSurveyRating surveyRating);

        void AddRating(DoctorSurveyRating rating);
    }
}