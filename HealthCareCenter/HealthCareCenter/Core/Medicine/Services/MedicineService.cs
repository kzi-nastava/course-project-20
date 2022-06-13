using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Core.Medicine.Repositories;

namespace HealthCareCenter.Core.Medicine.Services
{
    public class MedicineService
    {
        public static string GetName(int ID)
        {
            if (MedicineRepository.Medicines == null)
            {
                return null;
            }

            foreach (Models.Medicine medicine in MedicineRepository.Medicines)
            {
                if (medicine.ID == ID)
                {
                    return medicine.Name;
                }
            }

            return "";
        }
    }
}
