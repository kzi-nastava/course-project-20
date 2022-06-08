using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Model
{
    class DoctorSurveyRating
    {
        public int DoctorID { get; set; }
        public int PatientID { get; set; }
        public double Rating { get; set; }
        public string Comment { get; set; }

        public DoctorSurveyRating(int doctorID, int patientID, double rating, string comment)
        {
            DoctorID = doctorID;
            PatientID = patientID;
            Rating = rating;
            Comment = comment;
        }
    }
}
