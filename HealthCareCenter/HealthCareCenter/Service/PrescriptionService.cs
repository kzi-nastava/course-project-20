using HealthCareCenter.Enums;
using HealthCareCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace HealthCareCenter.Service
{
    public class PrescriptionService
    {
        public PrescriptionService(int _doctorID)
        {
            this._doctorID = _doctorID;
            SelectedMedicine = new List<Medicine>();
        }
        private List<DateTime>_times = new List<DateTime>();
        private MedicineInstruction _medicineInstruction;
        public List<Medicine> SelectedMedicine { get; set; }
        private int _doctorID;

        public bool ClearData()
        {
            _times.Clear();
            _medicineInstruction = null;
            SelectedMedicine.Clear();
            return true;
        }
        public bool AddTime(string hour,string minute) {
            try
            {
                DateTime time = DateTime.Parse(hour + ":" + minute);
                foreach(DateTime t in _times)
                {
                    if(t == time)
                    {
                        MessageBox.Show("This time has already been added");
                        return false;
                    }
                }
                _times.Add(time);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool CreateMedicineInstruction(int id,string comment,int dailyConsumption,ConsumptionPeriod consumptionPeriod) {
            if (SelectedMedicine.Count == 0)
            {
                MessageBox.Show("Add times at which the medicine should be taken");
                return false;
            }
            try
            {
                _medicineInstruction = new MedicineInstruction(++MedicineInstructionRepository.LargestID, comment, _times, dailyConsumption, consumptionPeriod);
                MedicineInstructionRepository.MedicineInstructions.Add(_medicineInstruction);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool CreateAPrescription()
        {
            List<int> medicineIDs = new List<int>();
            foreach(Medicine medicine in SelectedMedicine)
            {
                medicineIDs.Add(medicine.ID);
            }
            Prescription prescription = new Prescription(++PrescriptionRepository.LargestID,medicineIDs,_doctorID,_medicineInstruction.ID);
            
            PrescriptionRepository.Prescriptions.Add(prescription);
            return true;
        }
        public bool AddPrescription(Prescription prescription)
        {
            try
            {
                PrescriptionRepository.Prescriptions.Add(prescription);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
