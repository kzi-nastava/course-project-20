using HealthCareCenter.Model;
using HealthCareCenter.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealthCareCenter.Controller
{
    internal class DoctorSurveyOverviewController
    {
        public List<DoctorSurveyRating> GetDoctorSurveys()
        {
            return DoctorSurveyRatingRepository.Ratings;
        }

        public List<List<string>> GetBest3Doctors()
        {
            List<List<string>> doctors = new List<List<string>>();

            Dictionary<int, double> doctorsRaitings = DoctorSurveyRatingService.GetAllRatings();
            int i = 0;
            foreach (KeyValuePair<int, double> entry in doctorsRaitings)
            {
                Doctor doctor = DoctorService.Get(entry.Key);
                List<string> doctorAtributesForDisplay = new List<string>() { doctor.ID.ToString(), doctor.FirstName, doctor.LastName, entry.Value.ToString() };
                doctors.Add(doctorAtributesForDisplay);
                i++;
                if (i == 3)
                {
                    return doctors;
                }
            }
            return doctors;
        }

        public List<List<string>> GetWorst3Doctors()
        {
            List<List<string>> doctors = new List<List<string>>();

            Dictionary<int, double> doctorsRaitings = DoctorSurveyRatingService.GetAllRatings();
            int i = 0;
            foreach (KeyValuePair<int, double> entry in doctorsRaitings.Reverse())
            {
                Doctor doctor = DoctorService.Get(entry.Key);
                List<string> doctorAtributesForDisplay = new List<string>() { doctor.ID.ToString(), doctor.FirstName, doctor.LastName, entry.Value.ToString() };
                doctors.Add(doctorAtributesForDisplay);
                i++;
                if (i == 3)
                {
                    return doctors;
                }
            }

            return doctors;
        }
    }
}