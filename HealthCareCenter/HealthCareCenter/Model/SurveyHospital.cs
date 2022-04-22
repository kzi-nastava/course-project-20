using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Model
{
    public class SurveyHospital : Survey
    {
        public int Hygiene { get; set; }
        public int SatisfactionLevel { get; set; }
    }
}