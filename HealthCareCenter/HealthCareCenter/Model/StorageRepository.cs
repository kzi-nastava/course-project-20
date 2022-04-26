using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Model
{
    internal class StorageRepository
    {
        /// <summary>
        /// Get storage data
        /// </summary>
        /// <returns>Storage as room object</returns>
        public static Room GetStorage()
        {
            try
            {
                List<Room> storage = new List<Room>();
                var settings = new JsonSerializerSettings
                {
                    DateFormatString = Constants.DateFormat
                };

                string JSONTextStorage = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\Storage.json");
                storage = (List<Room>)JsonConvert.DeserializeObject<IEnumerable<Room>>(JSONTextStorage, settings);
                return storage[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}