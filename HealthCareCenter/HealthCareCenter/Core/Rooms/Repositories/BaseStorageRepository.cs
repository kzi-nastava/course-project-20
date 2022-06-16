using HealthCareCenter.Core.Rooms.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Rooms.Repositories
{
    public abstract class BaseStorageRepository
    {
        public abstract Room Load();
        public abstract bool Save(Room storage);
    }
}
