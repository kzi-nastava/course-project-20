using HealthCareCenter.Core.Rooms.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Rooms.Repositories
{
    public abstract class AHospitalRoomForRenovationRepository
    {
        public List<HospitalRoom> Rooms { get; set; }

        public abstract int GetLargestRoomId();

        protected abstract List<HospitalRoom> Load();

        public abstract bool Save();
    }
}