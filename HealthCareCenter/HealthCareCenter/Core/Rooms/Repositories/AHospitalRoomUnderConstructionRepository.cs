using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Rooms.Models
{
    public abstract class AHospitalRoomUnderConstructionRepository
    {
        public List<HospitalRoom> Rooms { get; set; }

        public abstract int GetLargestRoomId();

        protected abstract List<HospitalRoom> Load();

        public abstract bool Save();
    }
}