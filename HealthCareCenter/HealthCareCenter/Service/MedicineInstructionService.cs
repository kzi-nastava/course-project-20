using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Model;

namespace HealthCareCenter.Service
{
    class MedicineInstructionService
    {
        public static MedicineInstruction GetSingle(int ID)
        {
            if (MedicineInstructionRepository.MedicineInstructions == null)
            {
                return null;
            }

            foreach (MedicineInstruction instruction in MedicineInstructionRepository.MedicineInstructions)
            {
                if (instruction.ID == ID)
                {
                    return instruction;
                }
            }

            return null;
        }
    }
}
