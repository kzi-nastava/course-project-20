using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Model;

namespace HealthCareCenter.Service
{
    class MedicineService
    {
        public static string GetName(int ID)
        {
            if (MedicineRepository.Medicines == null)
            {
                return null;
            }

            foreach (Medicine medicine in MedicineRepository.Medicines)
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
