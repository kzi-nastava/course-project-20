using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Model
{
    public class HealthRecord
    {
        public int ID { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public List<string> PreviousDiseases { get; set; }
        public List<string> Allergens { get; set; }
        public int PatientID { get; set; }
        public List<int> AppointmentIDs { get; set; }

        public HealthRecord() { }
        public HealthRecord(int id, double height, double weight, List<string> previousDiseases, List<string> allergens, int patientID, List<int> appointmentIDs)
        {
            ID = id;
            Height = height;
            Weight = weight;
            PreviousDiseases = previousDiseases;
            Allergens = allergens;
            PatientID = patientID;
            AppointmentIDs = appointmentIDs;
        }
    }
}