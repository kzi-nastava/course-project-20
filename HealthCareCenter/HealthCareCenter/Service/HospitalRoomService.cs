using HealthCareCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Service
{
    internal class HospitalRoomService
    {
        /// <summary>
        /// Return loaded hospital rooms from list.
        /// </summary>
        /// <returns>Loaded hospital rooms.</returns>
        public static List<HospitalRoom> GetRooms()
        {
            return HospitalRoomRepository.Rooms;
        }

        /// <summary>
        /// Finding room with specific id.
        /// </summary>
        /// <param name="id">id of wanted hospital room.</param>
        /// <returns>Hospital room with specific id, if room is found, or null if room is not found.</returns>
        /// <exception cref="HospitalRoomNotFound">Thrown when room with specific id is not found.</exception>
        public static HospitalRoom Get(int id)
        {
            try
            {
                foreach (HospitalRoom room in HospitalRoomRepository.Rooms)
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

        /// <summary>
        /// Add new hospital room in file hospitalRooms.json.
        /// </summary>
        /// <param name="newRoom"></param>
        public static void Add(HospitalRoom newRoom)
        {
            HospitalRoomRepository.Rooms.Add(newRoom);
            HospitalRoomRepository.Save();
        }

        public static void Insert(HospitalRoom room)
        {
            HospitalRoomRepository.Rooms.Add(room);
            HospitalRoomRepository.Rooms.Sort((x, y) => x.ID.CompareTo(y.ID));
            HospitalRoomRepository.Save();
        }

        /// <summary>
        /// Finding last(largest) id in file hospitalRooms.json.
        /// </summary>
        /// <returns>last(largest) id.</returns>
        public static int GetLargestRoomId()
        {
            try
            {
                List<HospitalRoom> rooms = HospitalRoomRepository.Rooms;
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

        /// <summary>
        /// Delete room from file HospitalRooms.josn with specific id.
        /// </summary>
        /// <param name="id">id of the hospital room we want to delete.</param>
        /// <returns>True if room is deleted or false if it's not.</returns>
        /// <exception cref="HospitalRoomNotFound">Thrown when room with specific id is not found.</exception>
        public static bool Delete(int id)
        {
            try
            {
                for (int i = 0; i < HospitalRoomRepository.Rooms.Count; i++)
                {
                    if (id == HospitalRoomRepository.Rooms[i].ID)
                    {
                        HospitalRoomRepository.Rooms.RemoveAt(i);
                        HospitalRoomRepository.Save();
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
        public static bool Delete(HospitalRoom room)
        {
            try
            {
                for (int i = 0; i < HospitalRoomRepository.Rooms.Count; i++)
                {
                    if (room.ID == HospitalRoomRepository.Rooms[i].ID)
                    {
                        HospitalRoomRepository.Rooms.RemoveAt(i);
                        HospitalRoomRepository.Save();
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
        public static bool Update(HospitalRoom room)
        {
            try
            {
                for (int i = 0; i < HospitalRoomRepository.Rooms.Count; i++)
                {
                    if (room.ID == HospitalRoomRepository.Rooms[i].ID)
                    {
                        HospitalRoomRepository.Rooms[i] = room;
                        HospitalRoomRepository.Save();
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

        public static void Update(int roomID, Appointment appointment)
        {
            foreach (HospitalRoom room in HospitalRoomRepository.Rooms)
            {
                if (room.ID == roomID)
                {
                    room.AppointmentIDs.Add(appointment.ID);
                    return;
                }
            }
        }

        public static int GetAvailableRoomID(DateTime scheduledDate, Enums.RoomType roomType)
        {
            int hospitalRoomID = -1;
            foreach (HospitalRoom hospitalRoom in HospitalRoomRepository.Rooms)
            {
                if (hospitalRoom.Type != roomType)
                {
                    continue;
                }

                hospitalRoomID = hospitalRoom.ID;
                foreach (Appointment appointment in AppointmentRepository.Appointments)
                {
                    if (hospitalRoom.AppointmentIDs.Contains(appointment.ID))
                    {
                        if (appointment.ScheduledDate.CompareTo(scheduledDate) == 0)
                        {
                            hospitalRoomID = -1;
                        }
                    }
                }
                if (hospitalRoomID != -1)
                {
                    break;
                }
            }

            return hospitalRoomID;
        }

        public static void AddAppointmentToRoom(int hospitalRoomID, int appointmentID)
        {
            foreach (HospitalRoom hospitalRoom in HospitalRoomRepository.Rooms)
            {
                if (hospitalRoom.ID == hospitalRoomID)
                {
                    hospitalRoom.AppointmentIDs.Add(appointmentID);
                    break;
                }
            }
            HospitalRoomRepository.Save();
        }
    }
}