using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Model
{
    public class EquipmentRearrangementRepository
    {
        private static List<EquipmentRearrangement> s_rearrangements = LoadRearrangments();

        /// <summary>
        /// Loads all rearrangements from file EquipmentRearrangement.json.
        /// </summary>
        /// <returns>List of all rearrangements.</returns>
        private static List<EquipmentRearrangement> LoadRearrangments()
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
        /// Adding new rearrangement in file EquipmentRearrangement.json.
        /// </summary>
        /// <param name="newRearrangement"></param>
        public static void AddRearrangement(EquipmentRearrangement newRearrangement)
        {
            s_rearrangements.Add(newRearrangement);
            SaveAllRearrangements(s_rearrangements);
        }

        ///<summary>
        /// </summary>
        /// <returns>All loaded rearragements from list s_rearrangements.</returns>
        public static List<EquipmentRearrangement> GetRearrangments()
        {
            return s_rearrangements;
        }

        /// <summary>
        /// Finding rearragement with specific id
        /// </summary>
        /// <param name="id">Id of wanted rearragement</param>
        /// <returns>Equipment rearragement with specific id, if rearragement is found, or null if rearragement is not found</returns>
        /// <exception cref="EquipmentRearrangementNotFound">Thrown when rearragement with specific id is not found</exception>
        public static EquipmentRearrangement GetRearrangementById(int id)
        {
            try
            {
                foreach (EquipmentRearrangement rearrangement in s_rearrangements)
                {
                    if (rearrangement.ID == id)
                        return rearrangement;
                }

                throw new EquipmentRearrangementNotFound();
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

        /// <summary>
        /// Override content of file EquipmentRearrangement.json with rearrangements list.
        /// </summary>
        /// <param name="rearrangements"></param>
        /// <returns>True if content override is ended successfully.</returns>
        public static bool SaveAllRearrangements(List<EquipmentRearrangement> rearrangements)
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer();

                using (StreamWriter sw = new StreamWriter(@"..\..\..\data\EquipmentRearrangement.json"))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, rearrangements);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Finiding last (largest) id of equipment rearrangement.
        /// </summary>
        /// <returns>Last (largest) id of rearrangement</returns>
        public static int GetLastID()
        {
            try
            {
                List<EquipmentRearrangement> rearrangements = s_rearrangements;
                rearrangements.Sort((x, y) => x.ID.CompareTo(y.ID));
                if (rearrangements.Count == 0)
                    return -1;
                return rearrangements[rearrangements.Count - 1].ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Deleting equipment rearrangement with specific id.
        /// </summary>
        /// <param name="id">Equipment  rearrangement id</param>
        /// <returns>True if equipment rearrangement is deleted or false if is not</returns>
        /// <exception cref="EquipmentRearrangementNotFound">Thrown when equipment rearrangement not found</exception>
        public static bool DeleteRearrangement(int id)
        {
            try
            {
                for (int i = 0; i < s_rearrangements.Count; i++)
                {
                    if (id == s_rearrangements[i].ID)
                    {
                        s_rearrangements.RemoveAt(i);
                        SaveAllRearrangements(s_rearrangements);
                        return true;
                    }
                }
                throw new EquipmentRearrangementNotFound();
            }
            catch (EquipmentRearrangementNotFound ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Updating equipment rearrangemnt.
        /// </summary>
        /// <param name="rearrangement"></param>
        /// <returns>True if updating is performed or false if is not.</returns>
        /// <exception cref="EquipmentRearrangementNotFound">Thrown when equipment is not found.</exception>
        public static bool UpdateRearrangement(EquipmentRearrangement rearrangement)
        {
            try
            {
                for (int i = 0; i < s_rearrangements.Count; i++)
                {
                    if (rearrangement.ID == s_rearrangements[i].ID)
                    {
                        s_rearrangements[i] = rearrangement;
                        SaveAllRearrangements(s_rearrangements);
                        return true;
                    }
                }
                throw new EquipmentRearrangementNotFound();
            }
            catch (EquipmentRearrangementNotFound ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}