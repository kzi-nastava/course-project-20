using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.HealthRecords
{
    public abstract class BaseHealthRecordRepository
    {
        protected List<HealthRecord> _records;
        public List<HealthRecord> Records
        {
            get
            {
                if (_records == null)
                {
                    Load();
                }
                return _records;
            }
            set => _records = value;
        }
        public int LargestID { get; set; }

        public abstract void Load();
        public abstract void Save();
        public abstract int CalculateMaxID();

    }
}
