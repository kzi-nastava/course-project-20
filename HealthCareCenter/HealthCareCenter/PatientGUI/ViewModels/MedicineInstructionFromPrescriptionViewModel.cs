using HealthCareCenter.Model;
using HealthCareCenter.Service;

namespace HealthCareCenter.PatientGUI.ViewModels
{
    internal class MedicineInstructionFromPrescriptionViewModel : ViewModelBase
    {
        Prescription _prescription;
        MedicineInstruction _medicineInstruction;

        public string PrescriptionID => _prescription.ID.ToString();
        public string DoctorID => _prescription.DoctorID.ToString();
        public string DoctorName => UserService.GetUserFullName(_prescription.DoctorID);
        public string InstructionID => _medicineInstruction.ID.ToString();
        public string MedicineID => _medicineInstruction.MedicineID.ToString();
        public string MedicineName => MedicineService.GetName(_medicineInstruction.MedicineID);

        public MedicineInstructionFromPrescriptionViewModel(Prescription prescription, MedicineInstruction medicineInstruction)
        {
            _prescription = prescription;
            _medicineInstruction = medicineInstruction;
        }
    }
}
