using HealthCareCenter.Core.Rooms.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Rooms.Services
{
    public interface IHospitalRoomForRenovationService
    {
        List<HospitalRoom> GetRooms();

        HospitalRoom Get(int id);

        void Add(HospitalRoom newRoom);

        bool Delete(int id);

        bool Delete(HospitalRoom room);

        bool Update(HospitalRoom room);
    }
}