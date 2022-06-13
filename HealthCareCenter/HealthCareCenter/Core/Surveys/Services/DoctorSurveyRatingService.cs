using HealthCareCenter.Core.Surveys.Models;
using HealthCareCenter.Core.Surveys.Repositories;
using HealthCareCenter.Core.Users;
using HealthCareCenter.Core.Users.Models;
using System.Collections.Generic;
using System.Linq;

namespace HealthCareCenter.Core.Surveys.Services
{
    internal class DoctorSurveyRatingService
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

        public static Dictionary<int, double> GetAllRatings()
        {
            Dictionary<int, double> doctorsRatings = new Dictionary<int, double>();
            foreach (Doctor doctor in UserRepository.Doctors)
            {
                double rating = GetAverageRating(doctor.ID);
                if (rating > 0)
                {
                    doctorsRatings.Add(doctor.ID, GetAverageRating(doctor.ID));
                }
            }
            var orderedDoctorsRatings = doctorsRatings.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            return orderedDoctorsRatings;
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

        public static bool OverwriteExistingReview(DoctorSurveyRating surveyRating)
        {
            foreach (DoctorSurveyRating rating in DoctorSurveyRatingRepository.Ratings)
            {
                if (rating.DoctorID == surveyRating.DoctorID && rating.PatientID == surveyRating.PatientID)
                {
                    rating.Comment = surveyRating.Comment;
                    rating.Rating = surveyRating.Rating;
                    return true;
                }
            }

            return false;
        }
    }
}