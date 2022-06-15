using HealthCareCenter.Core.Medicine.Models;
using HealthCareCenter.Core.Medicine.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace HealthCareCenter.Core.Prescriptions
{
    public class PrescriptionService : BasePrescriptionService
    {
        private readonly BaseMedicineInstructionRepository _medicineInstructionRepository;
        private readonly BasePrescriptionRepository _prescriptionRepository;

        public PrescriptionService(
            BaseMedicineInstructionRepository medicineInstructionRepository,
            BasePrescriptionRepository prescriptionRepository)
        {
            _medicineInstructionRepository = medicineInstructionRepository;
            _prescriptionRepository = prescriptionRepository;
            MedicineInstructions = new List<int>();
            _times = new List<DateTime>();
        }

        public override void SetDoctorID(int doctorID)
        {
            _doctorID = doctorID;
        }

        public override bool ClearData(bool finishing)
        {
            _times.Clear();
            if (finishing)
                MedicineInstructions.Clear();
            SelectedMedicine = null;
            return true;
        }

        public override bool AddTime(string hour, string minute)
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

        public override bool CreateMedicineInstruction(int id, string comment, int dailyConsumption, ConsumptionPeriod consumptionPeriod, int medicineID)
        {
            bool sucessfull = CheckData(false);
            if (!sucessfull)
                return false;
            try
            {
                MedicineInstruction _medicineInstruction = new MedicineInstruction(++MedicineInstructionRepository.LargestID, comment, _times, dailyConsumption, consumptionPeriod, medicineID);
                _medicineInstructionRepository.MedicineInstructions.Add(_medicineInstruction);
                MedicineInstructions.Add(_medicineInstruction.ID);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public override bool Create()
        {
            bool sucessfull = CheckData(true);
            if (!sucessfull)
                return false;
            List<int> medicineInstructions = new List<int>(MedicineInstructions);
            Prescription prescription = new Prescription(++_prescriptionRepository.LargestID, _doctorID, medicineInstructions);
            _prescriptionRepository.Prescriptions.Add(prescription);
            return true;
        }

        protected override bool CheckData(bool finishing)
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

        public override bool Add(Prescription prescription)
        {
            try
            {
                _prescriptionRepository.Prescriptions.Add(prescription);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override List<Prescription> GetPatientPrescriptions(int healthRecordID)
        {
            if (_prescriptionRepository.Prescriptions == null)
            {
                return null;
            }

            List<Prescription> patientPrescriptions = new List<Prescription>();
            foreach (Prescription prescription in _prescriptionRepository.Prescriptions)
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
