using HealthCareCenter.Core.Equipment.Models;
using HealthCareCenter.Core.Equipment.Repositories;
using HealthCareCenter.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Equipment.Services
{
    public class EquipmentService : IEquipmentService
    {
        private readonly BaseEquipmentRepository _equipmentRepository;

        public EquipmentService(BaseEquipmentRepository equipmentRepository)
        {
            _equipmentRepository = equipmentRepository;
        }

        /// <summary>
        /// Finding equipment with specific id.
        /// </summary>
        /// <param name="id">id of wanted equipment</param>
        /// <returns>Equipment with specific id, if equipment is found, or null if equipment is not found.</returns>
        /// <exception cref="EquipmentNotFound">Thrown when equipment with specific id is not found.</exception>
        public Models.Equipment Get(int id)
        {
            try
            {
                foreach (Models.Equipment equipment in _equipmentRepository.Equipments)
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
        public List<Models.Equipment> GetEquipments()
        {
            return _equipmentRepository.Equipments;
        }

        /// <summary>
        /// Add new equipment in file equipments.json.
        /// </summary>
        /// <param name="newEquipment"></param>
        public void Add(Models.Equipment newEquipment)
        {
            _equipmentRepository.Equipments.Add(newEquipment);
            _equipmentRepository.Save();
        }

        /// <summary>
        /// Delete equipment from file Equipments.josn with specific id.
        /// </summary>
        /// <param name="id">id of the equipment we want to delete.</param>
        /// <returns>true if equipment is deleted or false if it's not.</returns>
        /// <exception cref="EquipmentNotFound">Thrown when equipment with specific id is not found.</exception>
        public bool Delete(int id)
        {
            try
            {
                for (int i = 0; i < _equipmentRepository.Equipments.Count; i++)
                {
                    if (id == _equipmentRepository.Equipments[i].ID)
                    {
                        _equipmentRepository.Equipments.RemoveAt(i);
                        _equipmentRepository.Save();
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
        public bool Delete(Models.Equipment equipment)
        {
            try
            {
                for (int i = 0; i < _equipmentRepository.Equipments.Count; i++)
                {
                    if (equipment.ID == _equipmentRepository.Equipments[i].ID)
                    {
                        _equipmentRepository.Equipments.RemoveAt(i);
                        _equipmentRepository.Save();
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
        public bool Update(Models.Equipment equipment)
        {
            try
            {
                for (int i = 0; i < _equipmentRepository.Equipments.Count; i++)
                {
                    if (equipment.ID == _equipmentRepository.Equipments[i].ID)
                    {
                        _equipmentRepository.Equipments[i] = equipment;
                        _equipmentRepository.Save();
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

        public bool HasScheduledRearrangement(Models.Equipment equipment)
        {
            return equipment.RearrangementID != -1;
        }
    }
}