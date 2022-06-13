using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Surveys.Models
{
    internal class DoctorSurveyRating : SurveyRating
    {
        public int DoctorID { get; set; }

        public DoctorSurveyRating(int doctorID, int patientID, string comment, double rating) :
            base(patientID, comment, rating)
        {
            DoctorID = doctorID;
        }

        public List<string> ToList()
        {
            return new List<string>() { DoctorID.ToString(), PatientID.ToString(), Comment, Rating.ToString() };
        }
    }
}