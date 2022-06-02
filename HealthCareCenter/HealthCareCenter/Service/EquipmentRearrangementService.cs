using HealthCareCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Service
{
    public static class EquipmentRearrangementService
    {
        /// <summary>
        /// Return all equipment rearrangements loaded in list.
        /// </summary>
        /// <returns>Loaded list of equipment rearrangements.</returns>
        public static List<EquipmentRearrangement> GetRearrangements()
        {
            return EquipmentRearrangementRepository.Rearrangements;
        }

        /// <summary>
        /// Adding new rearrangement in file equipmentRearrangement.json.
        /// </summary>
        /// <param name="newRearrangement"></param>
        public static void AddRearrangement(EquipmentRearrangement newRearrangement)
        {
            EquipmentRearrangementRepository.Rearrangements.Add(newRearrangement);
            EquipmentRearrangementRepository.Save();
        }

        /// <summary>
        /// Finding rearragement with specific id
        /// </summary>
        /// <param name="id">Id of wanted rearragement</param>
        /// <returns>Equipment rearragement with specific id, if rearragement is found, or null if rearragement is not found</returns>
        /// <exception cref="EquipmentRearrangementNotFound">Thrown when rearragement with specific id is not found</exception>
        public static EquipmentRearrangement GetRearrangement(int id)
        {
            try
            {
                foreach (EquipmentRearrangement rearrangement in EquipmentRearrangementRepository.Rearrangements)
                {
                    if (rearrangement.ID == id)
                    {
                        return rearrangement;
                    }
                }

                throw new EquipmentRearrangementNotFound();
            }
            catch (EquipmentRearrangementNotFound ex)
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
        /// Finiding last (largest) id of equipment rearrangement.
        /// </summary>
        /// <returns>Last (largest) id of rearrangement</returns>
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
        /// Deleting equipment rearrangement with specific id.
        /// </summary>
        /// <param name="id">Equipment  rearrangement id</param>
        /// <returns>True if equipment rearrangement is deleted or false if is not</returns>
        /// <exception cref="EquipmentRearrangementNotFound">Thrown when equipment rearrangement not found</exception>
        public static bool DeleteRearrangement(int id)
        {
            try
            {
                for (int i = 0; i < EquipmentRearrangementRepository.Rearrangements.Count; i++)
                {
                    if (id == EquipmentRearrangementRepository.Rearrangements[i].ID)
                    {
                        EquipmentRearrangementRepository.Rearrangements.RemoveAt(i);
                        EquipmentRearrangementRepository.Save();
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
                for (int i = 0; i < EquipmentRearrangementRepository.Rearrangements.Count; i++)
                {
                    if (rearrangement.ID == EquipmentRearrangementRepository.Rearrangements[i].ID)
                    {
                        EquipmentRearrangementRepository.Rearrangements[i] = rearrangement;
                        EquipmentRearrangementRepository.Save();
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