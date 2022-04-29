using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Model
{
    internal class EquipmentRepository
    {
        private static List<Equipment> s_equipments = LoadEquipments();

        /// <summary>
        /// Load all equipments from file Equipments.json.
        /// </summary>
        /// <returns>List of all hospital rooms.</returns>
        private static List<Equipment> LoadEquipments()
        {
            try
            {
                List<Equipment> equipments = new List<Equipment>();
                var settings = new JsonSerializerSettings
                {
                    DateFormatString = Constants.DateFormat
                };

                string JSONTextEquipments = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\Equipments.json");
                equipments = (List<Equipment>)JsonConvert.DeserializeObject<IEnumerable<Equipment>>(JSONTextEquipments, settings);
                return equipments;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Return loaded equipments from list.
        /// </summary>
        /// <returns>Loaded equipments.</returns>
        public static List<Equipment> GetEquipments()
        {
            return s_equipments;
        }

        /// <summary>
        /// Finding equipment with specific id.
        /// </summary>
        /// <param name="id">id of wanted equipment</param>
        /// <returns>Equipment with specific id, if equipment is found, or null if equipment is not found.</returns>
        /// <exception cref="EquipmentNotFound">Thrown when equipment with specific id is not found.</exception>
        public static Equipment GetEquipmentById(int id)
        {
            try
            {
                foreach (Equipment equipment in s_equipments)
                {
                    if (equipment.ID == id)
                        return equipment;
                }

                throw new EquipmentNotFound();
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
        /// Replace all data from file Equipments.json with list equipments.
        /// </summary>
        /// <param name="equipments">Data that will replace the old ones.</param>
        /// <returns>True if data update performed properly.</returns>
        public static bool SaveAllEquipments(List<Equipment> equipments)
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer();

                using (StreamWriter sw = new StreamWriter(@"..\..\..\data\Equipments.json"))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, equipments);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Add new equipment in file Equipments.json.
        /// </summary>
        /// <param name="newEquipment"></param>
        public static void AddEquipment(Equipment newEquipment)
        {
            s_equipments.Add(newEquipment);
            SaveAllEquipments(s_equipments);
        }

        /// <summary>
        /// Finding last(largest) id in file Equipments.json.
        /// </summary>
        /// <returns>last(largest) id.</returns>
        public static int GetLastEquipmentId()
        {
            try
            {
                List<Equipment> equipments = s_equipments;
                equipments.Sort((x, y) => x.ID.CompareTo(y.ID));
                if (equipments.Count == 0)
                    return -1;
                return equipments[equipments.Count - 1].ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Delete equipment from file Equipments.josn with specific id.
        /// </summary>
        /// <param name="id">id of the equipment we want to delete.</param>
        /// <returns>true if equipment is deleted or false if it's not.</returns>
        /// <exception cref="EquipmentNotFound">Thrown when equipment with specific id is not found.</exception>
        public static bool DeleteEquipment(int id)
        {
            try
            {
                for (int i = 0; i < s_equipments.Count; i++)
                {
                    if (id == s_equipments[i].ID)
                    {
                        s_equipments.RemoveAt(i);
                        SaveAllEquipments(s_equipments);
                        return true;
                    }
                }
                throw new EquipmentNotFound();
            }
            catch (EquipmentNotFound ex)
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
        /// Delete equipment from file Equipments.josn.
        /// </summary>
        /// <param name="equipment">equipment we want to delete</param>
        /// <returns>true if equipment is deleted or false if it's not</returns>
        /// <exception cref="EquipmentNotFound">Thrown when equipment with specific id is not found</exception>
        public static bool DeleteEquipment(Equipment equipment)
        {
            try
            {
                for (int i = 0; i < s_equipments.Count; i++)
                {
                    if (equipment.ID == s_equipments[i].ID)
                    {
                        s_equipments.RemoveAt(i);
                        SaveAllEquipments(s_equipments);
                        return true;
                    }
                }
                throw new EquipmentNotFound();
            }
            catch (EquipmentNotFound ex)
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
        /// Updating equipment.
        /// </summary>
        /// <param name="equipment"></param>
        /// <returns>true if equipment is found or false if it's not.</returns>
        /// <exception cref="EquipmentNotFound">Thrown when equipment is not found.</exception>
        public static bool UpdateEquipment(Equipment equipment)
        {
            try
            {
                for (int i = 0; i < s_equipments.Count; i++)
                {
                    if (equipment.ID == s_equipments[i].ID)
                    {
                        s_equipments[i] = equipment;
                        SaveAllEquipments(s_equipments);
                        return true;
                    }
                }
                throw new EquipmentNotFound();
            }
            catch (EquipmentNotFound ex)
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