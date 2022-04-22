using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Model
{
    public class Prescription
    {
        public int ID { get; set; }
        public List<int> MedicineIDs { get; set; }
        public int DoctorID { get; set; }
        public int MedicineInstructionID { get; set; }
    }
}