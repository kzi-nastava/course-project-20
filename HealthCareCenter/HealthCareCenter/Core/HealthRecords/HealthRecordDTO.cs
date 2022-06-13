using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.HealthRecords
{
    public class HealthRecordDTO
    {
        public int ID { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public List<string> PreviousDiseases { get; set; }
        public List<string> Allergens { get; set; }
        public int PatientID { get; set; }

        public HealthRecordDTO() { }
        public HealthRecordDTO(int id, string height, string weight, List<string> previousDiseases, List<string> allergens, int patientID)
        {
            ID = id;
            Height = height;
            Weight = weight;
            PreviousDiseases = previousDiseases;
            Allergens = allergens;
            PatientID = patientID;
        }
    }
}
