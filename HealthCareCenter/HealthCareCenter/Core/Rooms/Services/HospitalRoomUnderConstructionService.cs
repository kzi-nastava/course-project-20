using HealthCareCenter.Core.Exceptions;
using HealthCareCenter.Core.Rooms.Models;
using HealthCareCenter.Core.Rooms.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Rooms.Services
{
    public class HospitalRoomUnderConstructionService : IHospitalRoomUnderConstructionService
    {
        private AHospitalRoomUnderConstructionRepository _hospitalRoomUnderConstructionRepository;

        public HospitalRoomUnderConstructionService(AHospitalRoomUnderConstructionRepository hospitalRoomUnderConstructionRepository)
        {
            _hospitalRoomUnderConstructionRepository = hospitalRoomUnderConstructionRepository;
        }

        public List<HospitalRoom> GetRooms()
        {
            return _hospitalRoomUnderConstructionRepository.Rooms;
        }

        public HospitalRoom Get(int id)
        {
            try
            {
                foreach (HospitalRoom room in _hospitalRoomUnderConstructionRepository.Rooms)
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

        public void Add(HospitalRoom newRoom)
        {
            _hospitalRoomUnderConstructionRepository.Rooms.Add(newRoom);
            _hospitalRoomUnderConstructionRepository.Save();
        }

        public bool Delete(int id)
        {
            try
            {
                for (int i = 0; i < _hospitalRoomUnderConstructionRepository.Rooms.Count; i++)
                {
                    if (id == _hospitalRoomUnderConstructionRepository.Rooms[i].ID)
                    {
                        _hospitalRoomUnderConstructionRepository.Rooms.RemoveAt(i);
                        _hospitalRoomUnderConstructionRepository.Save();
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