using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Core.Medicine.Models;
using HealthCareCenter.Core.Medicine.Repositories;

namespace HealthCareCenter.Core.Medicine.Services
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

        public static string GetInfo(MedicineInstruction instruction)
        {
            string medicineInstructionInfo = "";
            medicineInstructionInfo += "Comment:\n";
            medicineInstructionInfo += "- " + instruction.Comment + "\n";
            medicineInstructionInfo += "Consumption time:\n";
            foreach (DateTime consumptionTime in instruction.ConsumptionTime)
            {
                medicineInstructionInfo += "- " + consumptionTime.ToString("t") + "h\n";
            }
            medicineInstructionInfo += "Daily consumption amount:\n";
            medicineInstructionInfo += "- " + instruction.DailyConsumption + "\n";
            medicineInstructionInfo += "Consumption period:\n";
            medicineInstructionInfo += "- " + instruction.ConsumptionPeriod;

            return medicineInstructionInfo;
        }
    }
}
