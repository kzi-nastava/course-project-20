using HealthCareCenter.Core.Rooms.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Rooms.Repositories
{
    public abstract class BaseHospitalRoomRepository
    {
        protected List<HospitalRoom> _rooms;
        public List<HospitalRoom> Rooms
        {
            get
            {
                if (_rooms == null)
                {
                    _ = Load();
                }

                return _rooms;
            }
            set => _rooms = value;
        }

        public abstract int GetLargestID();
        public abstract List<HospitalRoom> Load();
        public abstract bool Save();
    }
}
