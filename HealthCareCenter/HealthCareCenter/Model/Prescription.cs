using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Model
{
    public class Prescription
    {
        public Prescription() { }
        public Prescription(int _id, int _doctorID, List<int> _medicineInstructionsID) {
            this.ID = _id; 
            this.DoctorID = _doctorID;
            this.MedicineInstructionIDs = _medicineInstructionsID;
        }
        public int ID { get; set; }
        public int DoctorID { get; set; }
        public List<int> MedicineInstructionIDs { get; set; }

    }
}