using HealthCareCenter.Core.Surveys.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace HealthCareCenter.Core.Surveys.Repositories
{
    internal class DoctorSurveyRatingRepository : BaseDoctorSurveyRatingRepository
    {
        public override void Load()
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    DateFormatString = Constants.DateTimeFormat
                };

                string JSONTextRequests = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\doctorSurveyRatings.json");
                _ratings = (List<DoctorSurveyRating>)JsonConvert.DeserializeObject<IEnumerable<DoctorSurveyRating>>(JSONTextRequests, settings);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void Save()
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer
                {
                    Formatting = Formatting.Indented,
                    DateFormatString = Constants.DateTimeFormat
                };
                using (StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\doctorSurveyRatings.json"))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, Ratings);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}