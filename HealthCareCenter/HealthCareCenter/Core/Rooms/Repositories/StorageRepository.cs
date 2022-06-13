using HealthCareCenter.Core.Rooms.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Core.Rooms.Repositories
{
    internal class StorageRepository
    {
        private const string _fileName = "storage.json";

        /// <summary>
        /// Get storage data.
        /// </summary>
        /// <returns>Storage as room object.</returns>
        public static Room Load()
        {
            try
            {
                List<Room> storage = new List<Room>();
                var settings = new JsonSerializerSettings
                {
                    DateFormatString = Constants.DateFormat
                };

                string JSONTextStorage = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\Data\" + _fileName);
                storage = (List<Room>)JsonConvert.DeserializeObject<IEnumerable<Room>>(JSONTextStorage, settings);
                return storage[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Save storage data
        /// </summary>
        /// <param name="storage"></param>
        /// <returns></returns>
        public static bool Save(Room storage)
        {
            try
            {
                List<Room> rooms = new List<Room>();
                rooms.Add(storage);
                JsonSerializer serializer = new JsonSerializer
                {
                    Formatting = Formatting.Indented
                };

                using (StreamWriter sw = new StreamWriter(@"..\..\..\Data\" + _fileName))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, rooms);
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