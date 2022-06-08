using HealthCareCenter.Model;

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
