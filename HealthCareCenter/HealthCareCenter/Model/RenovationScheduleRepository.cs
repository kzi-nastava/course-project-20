﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Model
{
    internal class RenovationScheduleRepository
    {
        private const string _fileName = "renovationSchedules.json";
        public static List<RenovationSchedule> Renovations = Load();

        private static List<RenovationSchedule> Load()
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

        public static bool Save()
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