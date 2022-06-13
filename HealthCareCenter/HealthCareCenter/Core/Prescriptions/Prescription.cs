using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Prescriptions
{
    public class Prescription
    {
        public Prescription() { }
        public Prescription(int _id, int _doctorID, List<int> _medicineInstructionsID)
        {
            ID = _id;
            DoctorID = _doctorID;
            MedicineInstructionIDs = _medicineInstructionsID;
        }

        public Prescription(int _id, int _doctorID, List<int> _medicineInstructionsID, int _healthRecordID)
        {
            ID = _id;
            DoctorID = _doctorID;
            MedicineInstructionIDs = _medicineInstructionsID;
            HealthRecordID = _healthRecordID;
        }
        public int ID { get; set; }
        public int DoctorID { get; set; }
        public List<int> MedicineInstructionIDs { get; set; }
        public int HealthRecordID { get; set; }

    }
}