using HealthCareCenter.Core.Surveys.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Surveys.Repositories
{
    public abstract class BaseHealthcareSurveyRatingRepository
    {
        protected List<SurveyRating> _ratings;
        public List<SurveyRating> Ratings
        {
            get
            {
                if (_ratings == null)
                {
                    Load();
                }
                return _ratings;
            }
        }

        public abstract void Load();
        public abstract void Save();
    }
}
