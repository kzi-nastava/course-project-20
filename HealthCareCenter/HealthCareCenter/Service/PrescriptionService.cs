using HealthCareCenter.Enums;
using HealthCareCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace HealthCareCenter.Service
{
    public static class PrescriptionService
    {
        public static void Initialise(int _doctorID)
        {
            doctorID = _doctorID;
            MedicineInstructions = new List<int>();
        }
        private static List<DateTime>_times = new List<DateTime>();
        public static Medicine SelectedMedicine { get; set; }
        public static List<int> MedicineInstructions { get; set; }
        private static int doctorID;

        public static bool ClearData(bool finishing)
        {
            _times.Clear();
            if (finishing)
                MedicineInstructions.Clear();
            SelectedMedicine = null;
            return true;
        }
        public static bool AddTime(string hour,string minute) {
            try
            {
                DateTime time = DateTime.Parse(hour + ":" + minute);
                foreach (DateTime t in _times)
                {
                    if (t == time)
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
        public static bool CreateMedicineInstruction(int id,string comment,int dailyConsumption,ConsumptionPeriod consumptionPeriod, int medicineID) {
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
        public static bool CreateAPrescription()
        {
            bool sucessfull = checkData(true);
            if (!sucessfull)
                return false;
            List<int> medicineInstructions = new List<int>(MedicineInstructions);
            Prescription prescription = new Prescription(++PrescriptionRepository.LargestID, doctorID,medicineInstructions);
            PrescriptionRepository.Prescriptions.Add(prescription);
            return true;
        }

        private static bool checkData(bool finishing)
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
        public static bool AddPrescription(Prescription prescription)
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

        public static List<Prescription> GetPatientPrescriptions(int healthRecordID)
        {
            if (PrescriptionRepository.Prescriptions == null)
            {
                return null;
            }

            List<Prescription> patientPrescriptions = new List<Prescription>();
            foreach (Prescription prescription in PrescriptionRepository.Prescriptions)
            {
                if (prescription.HealthRecordID == healthRecordID)
                {
                    patientPrescriptions.Add(prescription);
                }
            }

            return patientPrescriptions;
        }
    }
}
