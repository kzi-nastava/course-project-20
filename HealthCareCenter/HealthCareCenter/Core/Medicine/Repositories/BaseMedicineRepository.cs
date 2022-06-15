using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Medicine.Repositories
{
    public abstract class BaseMedicineRepository
    {
        protected List<Models.Medicine> _medicines;
        public List<Models.Medicine> Medicines
        {
            get
            {
                if (_medicines == null)
                {
                    _ = Load();
                }
                return _medicines;
            }
            set => _medicines = value;
        }

        public abstract List<Models.Medicine> Load();
        public abstract void Save();
    }
}
