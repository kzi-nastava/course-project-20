using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Prescriptions
{
    public abstract class BasePrescriptionService
    {
        protected List<DateTime> _times;
        public Medicine.Models.Medicine SelectedMedicine { get; set; }
        public List<int> MedicineInstructions { get; set; }
        protected int _doctorID;

        public abstract void SetDoctorID(int doctorID);
        public abstract bool ClearData(bool finishing);
        public abstract bool AddTime(string hour, string minute);
        public abstract bool CreateMedicineInstruction(int id, string comment, int dailyConsumption, ConsumptionPeriod consumptionPeriod, int medicineID);
        public abstract bool Create();
        protected abstract bool CheckData(bool finishing);
        public abstract bool Add(Prescription prescription);
        public abstract List<Prescription> GetPatientPrescriptions(int healthRecordID);
    }
}
