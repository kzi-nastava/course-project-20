using HealthCareCenter.Core.Medicine.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Medicine.Repositories
{
    public abstract class BaseMedicineInstructionRepository
    {
        protected List<MedicineInstruction> _medicineInstructions;
        public List<MedicineInstruction> MedicineInstructions
        {
            get
            {
                if (_medicineInstructions == null)
                {
                    _ = Load();
                }
                return _medicineInstructions;
            }
            set => _medicineInstructions = value;
        }
        public static int LargestID { get; set; }

        public abstract List<MedicineInstruction> Load();
        public abstract void Save();
        public abstract int CalculateMaxID();
    }
}
