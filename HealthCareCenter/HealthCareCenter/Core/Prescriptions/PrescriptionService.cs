using HealthCareCenter.Core.Medicine.Models;
using HealthCareCenter.Core.Medicine.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace HealthCareCenter.Core.Prescriptions
{
    public static class PrescriptionService
    {
        public static void Initialise(int _doctorID)
        {
            doctorID = _doctorID;
            MedicineInstructions = new List<int>();
        }
        private static List<DateTime> _times = new List<DateTime>();
        public static Medicine.Models.Medicine SelectedMedicine { get; set; }
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
        public static bool AddTime(string hour, string minute)
        {
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
        public static bool CreateMedicineInstruction(int id, string comment, int dailyConsumption, ConsumptionPeriod consumptionPeriod, int medicineID)
        {
            bool sucessfull = CheckData(false);
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
        public static bool Create()
        {
            bool sucessfull = CheckData(true);
            if (!sucessfull)
                return false;
            List<int> medicineInstructions = new List<int>(MedicineInstructions);
            Prescription prescription = new Prescription(++PrescriptionRepository.LargestID, doctorID, medicineInstructions);
            PrescriptionRepository.Prescriptions.Add(prescription);
            return true;
        }

        private static bool CheckData(bool finishing)
        {
            if (_times.Count == 0 && !finishing)
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

        public static bool Add(Prescription prescription)
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
