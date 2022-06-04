using HealthCareCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Service
{
    internal class EquipmentService
    {
        /// <summary>
        /// Finding equipment with specific id.
        /// </summary>
        /// <param name="id">id of wanted equipment</param>
        /// <returns>Equipment with specific id, if equipment is found, or null if equipment is not found.</returns>
        /// <exception cref="EquipmentNotFound">Thrown when equipment with specific id is not found.</exception>
        public static Equipment Get(int id)
        {
            try
            {
                foreach (Equipment equipment in EquipmentRepository.Equipments)
                {
                    if (equipment.ID == id)
                    {
                        return equipment;
                    }
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
        /// Return loaded equipments from list.
        /// </summary>
        /// <returns>Loaded equipments.</returns>
        public static List<Equipment> GetEquipments()
        {
            return EquipmentRepository.Equipments;
        }

        /// <summary>
        /// Add new equipment in file equipments.json.
        /// </summary>
        /// <param name="newEquipment"></param>
        public static void Add(Equipment newEquipment)
        {
            EquipmentRepository.Equipments.Add(newEquipment);
            EquipmentRepository.Save();
        }

        /// <summary>
        /// Delete equipment from file Equipments.josn with specific id.
        /// </summary>
        /// <param name="id">id of the equipment we want to delete.</param>
        /// <returns>true if equipment is deleted or false if it's not.</returns>
        /// <exception cref="EquipmentNotFound">Thrown when equipment with specific id is not found.</exception>
        public static bool Delete(int id)
        {
            try
            {
                for (int i = 0; i < EquipmentRepository.Equipments.Count; i++)
                {
                    if (id == EquipmentRepository.Equipments[i].ID)
                    {
                        EquipmentRepository.Equipments.RemoveAt(i);
                        EquipmentRepository.Save();
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
        public static bool Delete(Equipment equipment)
        {
            try
            {
                for (int i = 0; i < EquipmentRepository.Equipments.Count; i++)
                {
                    if (equipment.ID == EquipmentRepository.Equipments[i].ID)
                    {
                        EquipmentRepository.Equipments.RemoveAt(i);
                        EquipmentRepository.Save();
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
        public static bool Update(Equipment equipment)
        {
            try
            {
                for (int i = 0; i < EquipmentRepository.Equipments.Count; i++)
                {
                    if (equipment.ID == EquipmentRepository.Equipments[i].ID)
                    {
                        EquipmentRepository.Equipments[i] = equipment;
                        EquipmentRepository.Save();
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

        public static bool HasScheduledRearrangement(Equipment equipment)
        {
            return equipment.RearrangementID != -1;
        }
    }
}