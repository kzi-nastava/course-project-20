using HealthCareCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Service
{
    internal class HospitalRoomUnderConstructionService
    {
        public static List<HospitalRoom> GetRooms()
        {
            return HospitalRoomUnderConstructionRepository.Rooms;
        }

        public static HospitalRoom GetRoom(int id)
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

        public static void AddRoom(HospitalRoom newRoom)
        {
            HospitalRoomUnderConstructionRepository.Rooms.Add(newRoom);
            HospitalRoomUnderConstructionRepository.SaveRooms(HospitalRoomUnderConstructionRepository.Rooms);
        }

        public static int GetLargestRoomId()
        {
            try
            {
                List<HospitalRoom> rooms = HospitalRoomUnderConstructionRepository.Rooms;
                rooms.Sort((x, y) => x.ID.CompareTo(y.ID));
                if (rooms.Count == 0)
                {
                    return 0;
                }

                return rooms[rooms.Count - 1].ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool DeleteRoom(int id)
        {
            try
            {
                for (int i = 0; i < HospitalRoomUnderConstructionRepository.Rooms.Count; i++)
                {
                    if (id == HospitalRoomUnderConstructionRepository.Rooms[i].ID)
                    {
                        HospitalRoomUnderConstructionRepository.Rooms.RemoveAt(i);
                        HospitalRoomUnderConstructionRepository.SaveRooms(HospitalRoomUnderConstructionRepository.Rooms);
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