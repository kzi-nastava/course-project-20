using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Model
{
    public class HospitalRoomRepository
    {
        private static List<HospitalRoom> s_rooms = LoadRooms();

        /// <summary>
        /// Loads all hospital rooms from file hospitalRooms.json.
        /// </summary>
        /// <returns>List of all hospital rooms.</returns>
        private static List<HospitalRoom> LoadRooms()
        {
            try
            {
                List<HospitalRoom> rooms = new List<HospitalRoom>();
                var settings = new JsonSerializerSettings
                {
                    DateFormatString = Constants.DateFormat
                };

                string JSONTextHospitalRooms = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\hospitalRooms.json");
                rooms = (List<HospitalRoom>)JsonConvert.DeserializeObject<IEnumerable<HospitalRoom>>(JSONTextHospitalRooms, settings);
                return rooms;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Return loaded hospital rooms from list.
        /// </summary>
        /// <returns>Loaded hospital rooms.</returns>
        public static List<HospitalRoom> GetRooms()
        {
            return s_rooms;
        }

        /// <summary>
        /// Finding room with specific id.
        /// </summary>
        /// <param name="id">id of wanted hospital room.</param>
        /// <returns>Hospital room with specific id, if room is found, or null if room is not found.</returns>
        /// <exception cref="HospitalRoomNotFound">Thrown when room with specific id is not found.</exception>
        public static HospitalRoom GetRoomById(int id)
        {
            try
            {
                foreach (HospitalRoom room in s_rooms)
                {
                    if (room.ID == id)
                        return room;
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

        /// <summary>
        /// Replace all data from file hospitalRooms.json with list rooms.
        /// </summary>
        /// <param name="rooms">Data that will replace the old ones.</param>
        /// <returns>true if data update performed properly.</returns>
        public static bool SaveAllRooms(List<HospitalRoom> rooms)
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer();

                using (StreamWriter sw = new StreamWriter(@"..\..\..\data\hospitalRooms.json"))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, rooms);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Add new hospital room in file hospitalRooms.json.
        /// </summary>
        /// <param name="newRoom"></param>
        public static void AddRoom(HospitalRoom newRoom)
        {
            s_rooms.Add(newRoom);
            SaveAllRooms(s_rooms);
        }

        /// <summary>
        /// Finding last(largest) id in file hospitalRooms.json.
        /// </summary>
        /// <returns>last(largest) id.</returns>
        public static int GetLastRoomId()
        {
            try
            {
                List<HospitalRoom> rooms = s_rooms;
                rooms.Sort((x, y) => x.ID.CompareTo(y.ID));
                if (rooms.Count == 0)
                    return 0;
                return rooms[rooms.Count - 1].ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Delete room from file HospitalRooms.josn with specific id.
        /// </summary>
        /// <param name="id">id of the hospital room we want to delete.</param>
        /// <returns>True if room is deleted or false if it's not.</returns>
        /// <exception cref="HospitalRoomNotFound">Thrown when room with specific id is not found.</exception>
        public static bool DeleteRoom(int id)
        {
            try
            {
                for (int i = 0; i < s_rooms.Count; i++)
                {
                    if (id == s_rooms[i].ID)
                    {
                        s_rooms.RemoveAt(i);
                        SaveAllRooms(s_rooms);
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

        /// <summary>
        /// Delete room from file HospitalRooms.josn with specific id.
        /// </summary>
        /// <param name="room">Room we want to delete.</param>
        /// <returns>true if room is deleted or false if it's not.</returns>
        /// <exception cref="HospitalRoomNotFound">Thrown when room is not found.</exception>
        public static bool DeleteRoom(HospitalRoom room)
        {
            try
            {
                for (int i = 0; i < s_rooms.Count; i++)
                {
                    if (room.ID == s_rooms[i].ID)
                    {
                        s_rooms.RemoveAt(i);
                        SaveAllRooms(s_rooms);
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

        /// <summary>
        /// Updating hospital room.
        /// </summary>
        /// <param name="room">Hospital room we want to update.</param>
        /// <returns>true if room is updated or false if room is not found.</returns>
        public static bool UpdateRoom(HospitalRoom room)
        {
            try
            {
                for (int i = 0; i < s_rooms.Count; i++)
                {
                    if (room.ID == s_rooms[i].ID)
                    {
                        s_rooms[i] = room;
                        SaveAllRooms(s_rooms);
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