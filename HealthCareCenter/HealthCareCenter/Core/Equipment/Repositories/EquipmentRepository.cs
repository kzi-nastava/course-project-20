using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace HealthCareCenter.Core.Equipment.Repositories
{
    internal class EquipmentRepository
    {
        private const string _fileName = "equipments.json";
        public static List<Models.Equipment> Equipments = Load();

        /// <summary>
        /// Finding last(largest) id in file equipments.json.
        /// </summary>
        /// <returns>last(largest) id.</returns>
        public static int GetLargestEquipmentId()
        {
            try
            {
                List<Models.Equipment> equipments = Equipments;
                equipments.Sort((x, y) => x.ID.CompareTo(y.ID));
                if (equipments.Count == 0)
                {
                    return -1;
                }

                return equipments[equipments.Count - 1].ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Load all equipments from file equipments.json.
        /// </summary>
        /// <returns>List of all hospital rooms.</returns>
        private static List<Models.Equipment> Load()
        {
            try
            {
                List<Models.Equipment> equipments = new List<Models.Equipment>();
                var settings = new JsonSerializerSettings
                {
                    DateFormatString = Constants.DateFormat
                };

                string JSONTextEquipments = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\Data\" + _fileName);
                equipments = (List<Models.Equipment>)JsonConvert.DeserializeObject<IEnumerable<Models.Equipment>>(JSONTextEquipments, settings);
                return equipments;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Replace all data from file equipments.json with list equipments.
        /// </summary>
        /// <param name="equipments">Data that will replace the old ones.</param>
        /// <returns>True if data update performed properly.</returns>
        public static bool Save()
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer();

                using (StreamWriter sw = new StreamWriter(@"..\..\..\Data\" + _fileName))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, Equipments);
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