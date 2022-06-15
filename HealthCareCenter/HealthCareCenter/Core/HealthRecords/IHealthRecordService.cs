using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Patients;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.HealthRecords
{
    public interface IHealthRecordService
    {
        HealthRecord GetRecordByPatientID(int id);
        HealthRecord Get(int id);
        HealthRecord Get(Patient patient);
        HealthRecord Get(Appointment appointment);
        string CheckAllergens(HealthRecord record);
        string CheckPreviousDiseases(HealthRecord record);
        void Update(double height, double weight, string[] previousDiseases, string[] allergens, int recordIndex);
        void FillPreviousDiseases(string[] previousDiseases, HealthRecord record);
        void FillAllergens(string[] allergens, HealthRecord record);
        string IsAllergicTo(Medicine.Models.Medicine medicine, HealthRecord record);
        void Delete(Patient patient);
    }
}
