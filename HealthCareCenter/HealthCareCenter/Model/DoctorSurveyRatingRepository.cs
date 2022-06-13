using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Model
{
    internal class DoctorSurveyRatingRepository
    {
        private static List<DoctorSurveyRating> _ratings;

        public static List<DoctorSurveyRating> Ratings
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

        public static void Load()
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

        public static void Save()
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