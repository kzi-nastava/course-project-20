using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Model
{
    public class Prescription
    {
        public Prescription() { }
        public Prescription(int _id, int _doctorID, Dictionary<int, int> _medicineInstructionsID) {
            this.ID = _id; 
            this.DoctorID = _doctorID;
            this.MedicineInstructions = _medicineInstructionsID;
        }
        public int ID { get; set; }
        public int DoctorID { get; set; }
        public Dictionary<int,int> MedicineInstructions { get; set; }
    }
}