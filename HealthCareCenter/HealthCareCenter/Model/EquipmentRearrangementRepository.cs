using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Model
{
    public class EquipmentRearrangementRepository
    {
        private const string _fileName = "equipmentRearrangement.json";
        public static List<EquipmentRearrangement> Rearrangements = Load();

        public static int GetLargestID()
        {
            try
            {
                List<EquipmentRearrangement> rearrangements = EquipmentRearrangementRepository.Rearrangements;
                rearrangements.Sort((x, y) => x.ID.CompareTo(y.ID));
                if (rearrangements.Count == 0)
                {
                    return -1;
                }

                return rearrangements[rearrangements.Count - 1].ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Loads all rearrangements from file equipmentRearrangement.json.
        /// </summary>
        /// <returns>List of all rearrangements.</returns>
        private static List<EquipmentRearrangement> Load()
        {
            try
            {
                List<EquipmentRearrangement> rearrangments = new List<EquipmentRearrangement>();
                var settings = new JsonSerializerSettings
                {
                    DateFormatString = Constants.DateFormat
                };

                string JSONTextEquipmentRearrangments = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\" + _fileName);
                rearrangments = (List<EquipmentRearrangement>)JsonConvert.DeserializeObject<IEnumerable<EquipmentRearrangement>>(JSONTextEquipmentRearrangments, settings);
                return rearrangments;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Override content of file equipmentRearrangement.json with rearrangements list.
        /// </summary>
        /// <param name="rearrangements"></param>
        /// <returns>True if content override is ended successfully.</returns>
        public static bool Save()
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer();

                using (StreamWriter sw = new StreamWriter(@"..\..\..\data\" + _fileName))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, Rearrangements);
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