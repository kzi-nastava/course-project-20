using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Model
{
    public class EquipmentRearrangementRepository
    {
        private static List<EquipmentRearrangement> s_rearrangements = getRearrangments();

        /// <summary>
        /// Loads all equipment rearrangement from file EquipmentRearrangement.json
        /// </summary>
        /// <returns>List of all equipment rearragements</returns>
        private static List<EquipmentRearrangement> getRearrangments()
        {
            try
            {
                List<EquipmentRearrangement> rearrangments = new List<EquipmentRearrangement>();
                var settings = new JsonSerializerSettings
                {
                    DateFormatString = Constants.DateFormat
                };

                string JSONTextEquipmentRearrangments = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\EquipmentRearrangement.json");
                rearrangments = (List<EquipmentRearrangement>)JsonConvert.DeserializeObject<IEnumerable<EquipmentRearrangement>>(JSONTextEquipmentRearrangments, settings);
                return rearrangments;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Return loaded equipment rearragements from list
        /// </summary>
        /// <returns>Loaded equipment rearragements</returns>
        public static List<EquipmentRearrangement> GetRearrangments()
        {
            return s_rearrangements;
        }

        /// <summary>
        /// Finding equipment rearragement with specific ID
        /// </summary>
        /// <param name="id">ID of wanted equipment rearragement</param>
        /// <returns>Equipment rearragement with specific ID, if equipment rearragement is found, or null if equipment rearragement is not found</returns>
        /// <exception cref="EquipmentRearrangementNotFount">Thrown when equipment rearragement with specific ID is not found</exception>
        public static EquipmentRearrangement GetRearrangementById(int id)
        {
            try
            {
                foreach (EquipmentRearrangement rearrangement in s_rearrangements)
                {
                    if (rearrangement.ID == id)
                        return rearrangement;
                }

                throw new EquipmentRearrangementNotFount();
            }
            catch (EquipmentNotFound ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}