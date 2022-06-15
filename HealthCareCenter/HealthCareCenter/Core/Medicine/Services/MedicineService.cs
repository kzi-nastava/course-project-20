using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Core.Medicine.Repositories;

namespace HealthCareCenter.Core.Medicine.Services
{
    public class MedicineService : IMedicineService
    {
        private readonly BaseMedicineRepository _medicineRepository;

        public MedicineService(BaseMedicineRepository medicineRepository)
        {
            _medicineRepository = medicineRepository;
        }

        public string GetName(int ID)
        {
            foreach (Models.Medicine medicine in _medicineRepository.Medicines)
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
