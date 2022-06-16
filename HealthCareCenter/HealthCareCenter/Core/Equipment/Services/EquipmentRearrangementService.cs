using HealthCareCenter.Core.Equipment.Models;
using HealthCareCenter.Core.Equipment.Repositories;
using HealthCareCenter.Core.Exceptions;
using HealthCareCenter.Core.Rooms;
using HealthCareCenter.Core.Rooms.Models;
using HealthCareCenter.Core.Rooms.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Equipment.Services
{
    public class EquipmentRearrangementService : IEquipmentRearrangementService
    {
        private readonly IRoomService _roomService;
        private readonly IEquipmentService _equipmentService;
        private readonly IHospitalRoomUnderConstructionService _hospitalRoomUnderConstructionService;

        public EquipmentRearrangementService(
            IRoomService roomService,
            IEquipmentService equipmentService,
            IHospitalRoomUnderConstructionService hospitalRoomUnderConstructionService)
        {
            _roomService = roomService;
            _equipmentService = equipmentService;
            _hospitalRoomUnderConstructionService = hospitalRoomUnderConstructionService;
        }

        private bool IsBeforeCurrentTime(DateTime rearrangementDate)
        {
            DateTime now = DateTime.Now;
            int value = DateTime.Compare(rearrangementDate, now);
            return value < 0;
        }

        public List<EquipmentRearrangement> GetRearrangements()
        {
            return EquipmentRearrangementRepository.Rearrangements;
        }

        public void Add(EquipmentRearrangement newRearrangement)
        {
            EquipmentRearrangementRepository.Rearrangements.Add(newRearrangement);
            EquipmentRearrangementRepository.Save();
        }

        public EquipmentRearrangement Get(int id)
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

        public bool Delete(int id)
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

        public bool Update(EquipmentRearrangement rearrangement)
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

        public void Remove(Models.Equipment equipment)
        {
            Room currentRoom = _roomService.Get(equipment.CurrentRoomID);

            if (_equipmentService.HasScheduledRearrangement(equipment))
            {
                // Get rearrangement
                EquipmentRearrangement rearrangement = Get(equipment.RearrangementID);
                Room newRoomOfRearrangement = _roomService.Get(equipment.CurrentRoomID);

                // Romeve rearrangement id from list RearrangemetIDs of newRoomOfPreviusRearrangement and currenRoom
                newRoomOfRearrangement.EquipmentRearrangementsIDs.Remove(rearrangement.ID);
                currentRoom.EquipmentRearrangementsIDs.Remove(rearrangement.ID);

                // Remove rearrangement from file
                Delete(rearrangement.ID);

                // Update rooms
                //********************************************
                _roomService.Update(newRoomOfRearrangement);
                _roomService.Update(currentRoom);
                //********************************************

                // Rearrangement removed
                equipment.RearrangementID = -1;
                _equipmentService.Update(equipment);
            }
        }

        public void Set(EquipmentRearrangement rearrangement, Models.Equipment equipment)
        {
            Room currentRoom = _roomService.Get(equipment.CurrentRoomID);
            Room newRoomOfCurrentRearrangement = _roomService.Get(rearrangement.NewRoomID);

            // Equipment already has rearrangemt
            if (_equipmentService.HasScheduledRearrangement(equipment))
            {
                Remove(equipment);
            }

            equipment.RearrangementID = rearrangement.ID;

            // Add new rearrangement id to list RearrangemetIDs of currenRoom and newRoomOfCurrentRearrangement
            currentRoom.EquipmentRearrangementsIDs.Add(rearrangement.ID);
            newRoomOfCurrentRearrangement.EquipmentRearrangementsIDs.Add(rearrangement.ID);

            // Add new rearrangemnt to file
            Add(rearrangement);

            // Update Rooms
            //********************************************
            _roomService.Update(newRoomOfCurrentRearrangement);
            _roomService.Update(currentRoom);
            //********************************************

            // Update equipment information
            _equipmentService.Update(equipment);
        }

        public void DoPossibleRearrangement(Models.Equipment equipment)
        {
            if (_equipmentService.HasScheduledRearrangement(equipment))
            {
                EquipmentRearrangement rearrangement = Get(equipment.RearrangementID);

                if (IsBeforeCurrentTime(rearrangement.MoveTime))
                {
                    Room currentRoom = _roomService.Get(equipment.CurrentRoomID);
                    Room newRoomOfRearrangement = _roomService.Get(rearrangement.NewRoomID);

                    // update data about equipment number in rooms
                    currentRoom.EquipmentAmounts[equipment.Name]--;

                    if (newRoomOfRearrangement.EquipmentAmounts.ContainsKey(equipment.Name))
                    {
                        newRoomOfRearrangement.EquipmentAmounts[equipment.Name]++;
                    }
                    else
                    {
                        newRoomOfRearrangement.EquipmentAmounts.Add(equipment.Name, 1);
                    }

                    _roomService.Update(currentRoom);
                    _roomService.Update(newRoomOfRearrangement);
                    equipment.CurrentRoomID = rearrangement.NewRoomID;
                    Remove(equipment);
                    _equipmentService.Update(equipment);
                }
            }
        }

        public bool IsIrrevocable(EquipmentRearrangement rearrangement)
        {
            List<HospitalRoom> rooms = _hospitalRoomUnderConstructionService.GetRooms();
            foreach (HospitalRoom room in rooms)
            {
                if (room.ID == rearrangement.OldRoomID || room.ID == rearrangement.NewRoomID)
                {
                    return true;
                }
            }

            return false;
        }
    }
}