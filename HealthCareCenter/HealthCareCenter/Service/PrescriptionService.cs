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
            MedicineInstructions = new List<int>();
        }
        private List<DateTime>_times = new List<DateTime>();
        public Medicine SelectedMedicine { get; set; }
        public  List<int> MedicineInstructions { get; set; }
        private int _doctorID;

        public bool ClearData(bool finishing)
        {
            _times.Clear();
            if (finishing)
                MedicineInstructions.Clear();
            SelectedMedicine = null;
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
        public bool CreateMedicineInstruction(int id,string comment,int dailyConsumption,ConsumptionPeriod consumptionPeriod, int medicineID) {
            bool sucessfull = checkData(false);
            if (!sucessfull)
                return false;
            try
            {
                MedicineInstruction _medicineInstruction = new MedicineInstruction(++MedicineInstructionRepository.LargestID, comment, _times, dailyConsumption, consumptionPeriod, medicineID);
                MedicineInstructionRepository.MedicineInstructions.Add(_medicineInstruction);
                MedicineInstructions.Add(_medicineInstruction.ID); 
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool CreateAPrescription()
        {
            bool sucessfull = checkData(true);
            if (!sucessfull)
                return false;
            List<int> medicineInstructions = new List<int>(MedicineInstructions);
            Prescription prescription = new Prescription(++PrescriptionRepository.LargestID,_doctorID,medicineInstructions);
            
            PrescriptionRepository.Prescriptions.Add(prescription);
            return true;
        }

        private bool checkData(bool finishing)
        {
            if(_times.Count == 0 && !finishing)
            {
                MessageBox.Show("Add a time");
                return false;
            }
            if (SelectedMedicine == null && !finishing)
            {
                MessageBox.Show("Select a medicine");
                return false;
            }
            if (MedicineInstructions.Count == 0 && finishing)
            {
                MessageBox.Show("Create a instruction for the medicine");
                return false;
            }
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
