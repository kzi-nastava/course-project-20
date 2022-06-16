using HealthCareCenter.Core.Surveys.Models;
using HealthCareCenter.Core.Surveys.Repositories;
using HealthCareCenter.Core.Surveys.Services;
using HealthCareCenter.Core.Users.Models;
using HealthCareCenter.Core.Users.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealthCareCenter.Core.Surveys.Controllers
{
    internal class DoctorSurveyOverviewController
    {
        private IDoctorSurveyRatingService _doctorSurveyRatingService;
        private readonly BaseDoctorSurveyRatingRepository _doctorSurveyRatingRepository;
        private readonly IDoctorService _doctorService;

        public DoctorSurveyOverviewController(
            IDoctorSurveyRatingService doctorSurveyRatingService, 
            BaseDoctorSurveyRatingRepository doctorSurveyRatingRepository,
            IDoctorService doctorService)
        {
            _doctorSurveyRatingService = doctorSurveyRatingService;
            _doctorSurveyRatingRepository = doctorSurveyRatingRepository;
            _doctorService = doctorService;
        }

        public List<DoctorSurveyRating> GetDoctorSurveys()
        {
            return _doctorSurveyRatingRepository.Ratings;
        }

        public List<List<string>> GetBest3Doctors()
        {
            List<List<string>> doctors = new List<List<string>>();

            Dictionary<int, double> doctorsRaitings = _doctorSurveyRatingService.GetAllRatings();
            int i = 0;
            foreach (KeyValuePair<int, double> entry in doctorsRaitings)
            {
                Doctor doctor = _doctorService.Get(entry.Key);
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

            Dictionary<int, double> doctorsRaitings = _doctorSurveyRatingService.GetAllRatings();
            int i = 0;
            foreach (KeyValuePair<int, double> entry in doctorsRaitings.Reverse())
            {
                Doctor doctor = _doctorService.Get(entry.Key);
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