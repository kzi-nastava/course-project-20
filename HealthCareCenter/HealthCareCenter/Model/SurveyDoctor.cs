using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Model
{
    public class SurveyDoctor : Survey
    {
        public int DoctorID { get; set; }
        public int ServiceQuality { get; set; }
    }
}