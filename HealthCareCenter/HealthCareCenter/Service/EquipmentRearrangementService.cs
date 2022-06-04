using HealthCareCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Service
{
    public static class EquipmentRearrangementService
    {
        private static bool IsBeforeCurrentTime(DateTime rearrangementDate)
        {
            DateTime now = DateTime.Now;
            int value = DateTime.Compare(rearrangementDate, now);
            return value < 0;
        }

        public static List<EquipmentRearrangement> GetRearrangements()
        {
            return EquipmentRearrangementRepository.Rearrangements;
        }

        public static void Add(EquipmentRearrangement newRearrangement)
        {
            EquipmentRearrangementRepository.Rearrangements.Add(newRearrangement);
            EquipmentRearrangementRepository.Save();
        }

        public static EquipmentRearrangement Get(int id)
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

        public static bool Delete(int id)
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

        public static bool Update(EquipmentRearrangement rearrangement)
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

        public static void Remove(Equipment equipment)
        {
            Room currentRoom = RoomService.Get(equipment.CurrentRoomID);

            if (EquipmentService.HasScheduledRearrangement(equipment))
            {
                // Get rearrangement
                EquipmentRearrangement rearrangement = EquipmentRearrangementService.Get(equipment.RearrangementID);
                Room newRoomOfRearrangement = RoomService.Get(equipment.CurrentRoomID);

                // Romeve rearrangement id from list RearrangemetIDs of newRoomOfPreviusRearrangement and currenRoom
                newRoomOfRearrangement.EquipmentRearrangementsIDs.Remove(rearrangement.ID);
                currentRoom.EquipmentRearrangementsIDs.Remove(rearrangement.ID);

                // Remove rearrangement from file
                EquipmentRearrangementService.Delete(rearrangement.ID);

                // Update rooms
                //********************************************
                RoomService.Update(newRoomOfRearrangement);
                RoomService.Update(currentRoom);
                //********************************************

                // Rearrangement removed
                equipment.RearrangementID = -1;
                EquipmentService.Update(equipment);
            }
        }

        public static void Set(EquipmentRearrangement rearrangement, Equipment equipment)
        {
            Room currentRoom = RoomService.Get(equipment.CurrentRoomID);
            Room newRoomOfCurrentRearrangement = RoomService.Get(rearrangement.NewRoomID);

            // Equipment already has rearrangemt
            if (EquipmentService.HasScheduledRearrangement(equipment))
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
            RoomService.Update(newRoomOfCurrentRearrangement);
            RoomService.Update(currentRoom);
            //********************************************

            // Update equipment information
            EquipmentService.Update(equipment);
        }

        public static void DoPossibleRearrangement(Equipment equipment)
        {
            if (EquipmentService.HasScheduledRearrangement(equipment))
            {
                EquipmentRearrangement rearrangement = EquipmentRearrangementService.Get(equipment.RearrangementID);

                if (IsBeforeCurrentTime(rearrangement.MoveTime))
                {
                    Room currentRoom = RoomService.Get(equipment.CurrentRoomID);
                    Room newRoomOfRearrangement = RoomService.Get(rearrangement.NewRoomID);

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

                    RoomService.Update(currentRoom);
                    RoomService.Update(newRoomOfRearrangement);
                    equipment.CurrentRoomID = rearrangement.NewRoomID;
                    Remove(equipment);
                    EquipmentService.Update(equipment);
                }
            }
        }

        public static bool IsIrrevocable(EquipmentRearrangement rearrangement)
        {
            List<HospitalRoom> rooms = HospitalRoomUnderConstructionService.GetRooms();
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