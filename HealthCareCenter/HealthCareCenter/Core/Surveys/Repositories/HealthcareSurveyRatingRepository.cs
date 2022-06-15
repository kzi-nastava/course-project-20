using HealthCareCenter.Core;
using HealthCareCenter.Core.Surveys.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Core.Surveys.Repositories
{
    public class HealthcareSurveyRatingRepository : BaseHealthcareSurveyRatingRepository
    {
        public override void Load()
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    DateFormatString = Constants.DateTimeFormat
                };

                string JSONTextRequests = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\healthcareSurveyRatings.json");
                _ratings = (List<SurveyRating>)JsonConvert.DeserializeObject<IEnumerable<SurveyRating>>(JSONTextRequests, settings);
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
                using (StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\healthcareSurveyRatings.json"))
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
