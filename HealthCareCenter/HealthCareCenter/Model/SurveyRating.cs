using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Model
{
    class SurveyRating
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
    }
}
