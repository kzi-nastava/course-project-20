using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Core.Medicine.Models;
using HealthCareCenter.Core.Medicine.Repositories;

namespace HealthCareCenter.Core.Medicine.Services
{
    public class MedicineInstructionService : IMedicineInstructionService
    {
        private readonly BaseMedicineInstructionRepository _medicineInstructionRepository;

        public MedicineInstructionService(BaseMedicineInstructionRepository medicineInstructionRepository)
        {
            _medicineInstructionRepository = medicineInstructionRepository;
        }

        public MedicineInstruction GetSingle(int ID)
        {
            foreach (MedicineInstruction instruction in _medicineInstructionRepository.MedicineInstructions)
            {
                if (instruction.ID == ID)
                {
                    return instruction;
                }
            }

            return null;
        }

        public string GetInfo(MedicineInstruction instruction)
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
