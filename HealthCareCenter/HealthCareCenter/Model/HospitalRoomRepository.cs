using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Model
{
    public static class HospitalRoomRepository
    {
        private const string _fileName = "hospitalRooms.json";
        public static List<HospitalRoom> Rooms = Load();

        /// <summary>
        /// Loads all hospital rooms from file hospitalRooms.json.
        /// </summary>
        /// <returns>List of all hospital rooms.</returns>
        public static List<HospitalRoom> Load()
        {
            try
            {
                List<HospitalRoom> rooms = new List<HospitalRoom>();
                var settings = new JsonSerializerSettings
                {
                    DateFormatString = Constants.DateFormat
                };

                string JSONTextHospitalRooms = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\" + _fileName);
                rooms = (List<HospitalRoom>)JsonConvert.DeserializeObject<IEnumerable<HospitalRoom>>(JSONTextHospitalRooms, settings);
                Rooms = rooms;
                return rooms;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Replace all data from file hospitalRooms.json with list rooms.
        /// </summary>
        /// <param name="rooms">Data that will replace the old ones.</param>
        /// <returns>true if data update performed properly.</returns>
        public static bool Save()
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer
                {
                    Formatting = Formatting.Indented
                };

                using (StreamWriter sw = new StreamWriter(@"..\..\..\data\" + _fileName))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, Rooms);
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