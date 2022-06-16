using HealthCareCenter.Core.Rooms.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Rooms.Services
{
    public interface IHospitalRoomUnderConstructionService
    {
        List<HospitalRoom> GetRooms();

        HospitalRoom Get(int id);

        void Add(HospitalRoom newRoom);

        bool Delete(int id);
    }
}