using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Prescriptions
{
    public abstract class BasePrescriptionRepository
    {
        protected List<Prescription> _prescriptions;
        public List<Prescription> Prescriptions
        {
            get
            {
                if (_prescriptions == null)
                {
                    _ = Load();
                }
                return _prescriptions;
            }
            set => _prescriptions = value;
        }
        public int LargestID { get; set; }

        public abstract List<Prescription> Load();
        public abstract void Save();
    }
}
