﻿using HealthCareCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Service
{
    internal class RoomService
    {
        /// <summary>
        /// Get every equipment amount like dictionary, where key is name of equipment and value amount.
        /// </summary>
        /// <returns>equipments amount</returns>
        public static Dictionary<string, int> GetEquipmentsAmount()
        {
            List<HospitalRoom> rooms = HospitalRoomService.GetRooms();
            Room storage = StorageRepository.Load();

            List<Room> hospitalPremises = new List<Room>();

            hospitalPremises.Add(storage);
            foreach (HospitalRoom room in rooms)
            {
                hospitalPremises.Add(room);
            }

            Dictionary<string, int> equipmentsAmount = new Dictionary<string, int>();
            foreach (Room room in hospitalPremises)
            {
                foreach (KeyValuePair<string, int> entry in room.EquipmentAmounts)
                {
                    if (equipmentsAmount.ContainsKey(entry.Key))
                    {
                        equipmentsAmount[entry.Key] = equipmentsAmount[entry.Key] + entry.Value;
                    }
                    else
                    {
                        equipmentsAmount[entry.Key] = entry.Value;
                    }
                }
            }
            return equipmentsAmount;
        }

        /// <summary>
        /// Update room by type, if is storage than update storage or if is hospital room than update hospital room
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public static bool Update(Room room)
        {
            try
            {
                if (room.IsStorage())
                {
                    StorageRepository.Save(room);
                }
                else
                {
                    HospitalRoom hospitalRoom = (HospitalRoom)room;
                    if (hospitalRoom.IsCurrentlyRenovating())
                    {
                        HospitalRoomForRenovationService.Update(hospitalRoom);
                    }
                    else
                    {
                        HospitalRoomService.Update(hospitalRoom);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Room Get(int roomId)
        {
            try
            {
                Room room = null;
                if (roomId == 0)
                {
                    room = StorageRepository.Load();
                }
                if (room == null)
                {
                    room = HospitalRoomService.Get(roomId);
                }
                if (room == null)
                {
                    room = HospitalRoomForRenovationService.Get(roomId);
                }
                if (room == null)
                {
                    room = HospitalRoomUnderConstructionService.Get(roomId);
                }
                if (room == null)
                {
                    throw new HospitalPremisesNotFound();
                }

                return room;
            }
            catch (HospitalPremisesNotFound ex)
            {
                Console.WriteLine(ex);
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}