using HealthCareCenter.Core.Medicine.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Medicine.Services
{
    public interface IMedicineInstructionService
    {
        MedicineInstruction GetSingle(int ID);
        string GetInfo(MedicineInstruction instruction);
    }
}
