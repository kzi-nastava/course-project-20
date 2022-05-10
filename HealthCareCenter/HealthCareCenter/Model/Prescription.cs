using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Model
{
    public class Prescription
    {
        public Prescription() { }
        public Prescription(int _id, List<int> _medicineIDs, int _doctorID, int _medicineInstructionID) {
            this.ID = _id; 
            this.MedicineIDs = _medicineIDs;
            this.DoctorID = _doctorID;
            this.MedicineInstructionID = _medicineInstructionID;
        }
        public int ID { get; set; }
        public List<int> MedicineIDs { get; set; }
        public int DoctorID { get; set; }
        public int MedicineInstructionID { get; set; }
    }
}