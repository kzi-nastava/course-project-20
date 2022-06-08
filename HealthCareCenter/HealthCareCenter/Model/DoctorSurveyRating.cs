using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Model
{
    class DoctorSurveyRating : SurveyRating
    {
        public int DoctorID { get; set; }

        public DoctorSurveyRating(int doctorID, int patientID, string comment, double rating) :
            base(patientID, comment, rating)
        {
            DoctorID = doctorID;
        }
    }
}
