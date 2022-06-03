using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Model
{
    public class Prescription
    {
        public Prescription() { }
        public Prescription(int _id, int _doctorID, List<int> _medicineInstructionsID)
        {
            this.ID = _id;
            this.DoctorID = _doctorID;
            this.MedicineInstructionIDs = _medicineInstructionsID;
        }

        public Prescription(int _id, int _doctorID, List<int> _medicineInstructionsID, int _healthRecordID)
        {
            this.ID = _id;
            this.DoctorID = _doctorID;
            this.MedicineInstructionIDs = _medicineInstructionsID;
            this.HealthRecordID = _healthRecordID;
        }
        public int ID { get; set; }
        public int DoctorID { get; set; }
        public List<int> MedicineInstructionIDs { get; set; }
        public int HealthRecordID { get; set; }

    }
}