using HealthCareCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Service
{
    class DoctorSurveyRatingService
    {
        public static double GetAverageRating(int doctorID)
        {
            double average = 0.0;
            double count = 0.0;
            foreach (DoctorSurveyRating rating in DoctorSurveyRatingRepository.Ratings)
            {
                if (rating.DoctorID == doctorID)
                {
                    average += rating.Rating;
                    ++count;
                }
            }

            return count == 0.0 ? 0.0 : average / count;
        }

        public static bool HasPatientAlreadyReviewed(int patientID, int doctorID)
        {
            foreach (DoctorSurveyRating rating in DoctorSurveyRatingRepository.Ratings)
            {
                if (patientID == rating.PatientID && doctorID == rating.DoctorID)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
