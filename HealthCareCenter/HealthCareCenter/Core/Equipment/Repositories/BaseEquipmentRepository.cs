using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Equipment.Repositories
{
    public abstract class BaseEquipmentRepository
    {
        protected List<Models.Equipment> _equipments;
        public List<Models.Equipment> Equipments
        {
            get
            {
                if (_equipments == null)
                {
                    _ = Load();
                }
                return _equipments;
            }
            set => _equipments = value;
        }

        public abstract int GetLargestID();
        public abstract List<Models.Equipment> Load();
        public abstract bool Save();
    }
}
