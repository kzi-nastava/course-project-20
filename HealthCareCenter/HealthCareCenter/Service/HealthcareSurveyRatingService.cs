using HealthCareCenter.Model;

namespace HealthCareCenter.Service
{
    class HealthcareSurveyRatingService
    {
        public static double GetAverageRating()
        {
            double average = 0.0;
            double count = 0.0;
            foreach (SurveyRating rating in HealthcareSurveyRatingRepository.Ratings)
            {
                average += rating.Rating;
                ++count;
            }

            return count == 0.0 ? 0.0 : average / count;
        }

        public static bool HasPatientAlreadyReviewed(int patientID)
        {
            foreach (SurveyRating rating in HealthcareSurveyRatingRepository.Ratings)
            {
                if (patientID == rating.PatientID)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool OverwriteExistingReview(SurveyRating surveyRating)
        {
            foreach (SurveyRating rating in HealthcareSurveyRatingRepository.Ratings)
            {
                if (rating.PatientID == surveyRating.PatientID)
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
