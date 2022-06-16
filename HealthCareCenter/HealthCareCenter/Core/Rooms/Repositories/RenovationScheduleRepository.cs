using HealthCareCenter.Core.Rooms.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Core.Rooms.Repositories
{
    public class RenovationScheduleRepository : ARenovationScheduleRepository
    {
        private const string _fileName = "renovationSchedules.json";

        public RenovationScheduleRepository()
        {
            Renovations = Load();
        }

        public override int GetLargestId()
        {
            try
            {
                List<RenovationSchedule> renovations = Renovations;
                renovations.Sort((x, y) => x.ID.CompareTo(y.ID));
                if (renovations.Count == 0)
                {
                    return 0;
                }

                return renovations[renovations.Count - 1].ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override List<RenovationSchedule> Load()
        {
            try
            {
                List<RenovationSchedule> rooms = new List<RenovationSchedule>();
                var settings = new JsonSerializerSettings
                {
                    DateFormatString = Constants.DateFormat
                };

                string JSONTextRenovations = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\" + _fileName);
                rooms = (List<RenovationSchedule>)JsonConvert.DeserializeObject<IEnumerable<RenovationSchedule>>(JSONTextRenovations, settings);
                return rooms;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override bool Save()
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer();

                using (StreamWriter sw = new StreamWriter(@"..\..\..\data\" + _fileName))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, Renovations);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}