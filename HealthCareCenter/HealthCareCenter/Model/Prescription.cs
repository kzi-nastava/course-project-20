using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Model
{
    public class Prescription
    {
        public Prescription() { }
        public Prescription(int _id, int _doctorID, Dictionary<int, int> _medicineInstructionIDs)
        {
            this.ID = _id;
            this.DoctorID = _doctorID;
            this.MedicineInstructions = _medicineInstructionIDs;
        }
        public int ID { get; set; }
        public int DoctorID { get; set; }
        public Dictionary<int, int> MedicineInstructions { get; set; }
        public List<int> MedicineInstructionIDs { get; set; }
        public int HealthRecordID { get; set; }
    }
}