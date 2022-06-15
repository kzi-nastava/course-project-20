﻿using HealthCareCenter.Core.Exceptions;
using HealthCareCenter.Core.Rooms.Models;
using HealthCareCenter.Core.Rooms.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Rooms.Services
{
    internal class HospitalRoomUnderConstructionService
    {
        public static List<HospitalRoom> GetRooms()
        {
            return HospitalRoomUnderConstructionRepository.Rooms;
        }

        public static HospitalRoom Get(int id)
        {
            try
            {
                foreach (HospitalRoom room in HospitalRoomUnderConstructionRepository.Rooms)
                {
                    if (room.ID == id)
                    {
                        return room;
                    }
                }

                throw new HospitalRoomNotFound();
            }
            catch (HospitalRoomNotFound ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Add(HospitalRoom newRoom)
        {
            HospitalRoomUnderConstructionRepository.Rooms.Add(newRoom);
            HospitalRoomUnderConstructionRepository.Save();
        }

        public static bool Delete(int id)
        {
            try
            {
                for (int i = 0; i < HospitalRoomUnderConstructionRepository.Rooms.Count; i++)
                {
                    if (id == HospitalRoomUnderConstructionRepository.Rooms[i].ID)
                    {
                        HospitalRoomUnderConstructionRepository.Rooms.RemoveAt(i);
                        HospitalRoomUnderConstructionRepository.Save();
                        return true;
                    }
                }
                throw new HospitalRoomNotFound();
            }
            catch (HospitalRoomNotFound ex)
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