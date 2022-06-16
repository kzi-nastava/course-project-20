using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Surveys.Models
{
    public class SurveyRating
    {
        public int PatientID { get; set; }
        public string Comment { get; set; }
        public double Rating { get; set; }

        public SurveyRating(int patientID, string comment, double rating)
        {
            PatientID = patientID;
            Comment = comment;
            Rating = rating;
        }

        public virtual List<string> ToList()
        {
            return new List<string>() { PatientID.ToString(), Comment, Rating.ToString() };
        }
    }
}