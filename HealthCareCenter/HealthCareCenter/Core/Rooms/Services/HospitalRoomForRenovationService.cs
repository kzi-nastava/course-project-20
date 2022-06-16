using HealthCareCenter.Core.Exceptions;
using HealthCareCenter.Core.Rooms.Models;
using HealthCareCenter.Core.Rooms.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Rooms.Services
{
    public class HospitalRoomForRenovationService : IHospitalRoomForRenovationService
    {
        private BaseHospitalRoomForRenovationRepository _hospitalRoomForRenovationRepository;

        public HospitalRoomForRenovationService(BaseHospitalRoomForRenovationRepository hospitalRoomForRenovationRepository)
        {
            _hospitalRoomForRenovationRepository = hospitalRoomForRenovationRepository;
        }

        public List<HospitalRoom> GetRooms()
        {
            return _hospitalRoomForRenovationRepository.Rooms;
        }

        /// <summary>
        /// Finding room with specific id.
        /// </summary>
        /// <param name="id">id of wanted hospital room.</param>
        /// <returns>Hospital room with specific id, if room is found, or null if room is not found.</returns>
        /// <exception cref="HospitalRoomNotFound">Thrown when room with specific id is not found.</exception>
        public HospitalRoom Get(int id)
        {
            try
            {
                foreach (HospitalRoom room in _hospitalRoomForRenovationRepository.Rooms)
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
        /// Add new hospital room in file hospitalRoomsForRenovation.json.
        /// </summary>
        /// <param name="newRoom"></param>
        public void Add(HospitalRoom newRoom)
        {
            _hospitalRoomForRenovationRepository.Rooms.Add(newRoom);
            _hospitalRoomForRenovationRepository.Save();
        }

        /// <summary>
        /// Delete room from file hospitalRoomsForRenovation.josn with specific id.
        /// </summary>
        /// <param name="id">id of the hospital room we want to delete.</param>
        /// <returns>True if room is deleted or false if it's not.</returns>
        /// <exception cref="HospitalRoomNotFound">Thrown when room with specific id is not found.</exception>
        public bool Delete(int id)
        {
            try
            {
                for (int i = 0; i < _hospitalRoomForRenovationRepository.Rooms.Count; i++)
                {
                    if (id == _hospitalRoomForRenovationRepository.Rooms[i].ID)
                    {
                        _hospitalRoomForRenovationRepository.Rooms.RemoveAt(i);
                        _hospitalRoomForRenovationRepository.Save();
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
        /// Delete room from file hospitalRoomsForRenovation.josn with specific id.
        /// </summary>
        /// <param name="room">Room we want to delete.</param>
        /// <returns>true if room is deleted or false if it's not.</returns>
        /// <exception cref="HospitalRoomNotFound">Thrown when room is not found.</exception>
        public bool Delete(HospitalRoom room)
        {
            try
            {
                for (int i = 0; i < _hospitalRoomForRenovationRepository.Rooms.Count; i++)
                {
                    if (room.ID == _hospitalRoomForRenovationRepository.Rooms[i].ID)
                    {
                        _hospitalRoomForRenovationRepository.Rooms.RemoveAt(i);
                        _hospitalRoomForRenovationRepository.Save();
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

        public bool Update(HospitalRoom room)
        {
            try
            {
                for (int i = 0; i < _hospitalRoomForRenovationRepository.Rooms.Count; i++)
                {
                    if (room.ID == _hospitalRoomForRenovationRepository.Rooms[i].ID)
                    {
                        _hospitalRoomForRenovationRepository.Rooms[i] = room;
                        _hospitalRoomForRenovationRepository.Save();
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