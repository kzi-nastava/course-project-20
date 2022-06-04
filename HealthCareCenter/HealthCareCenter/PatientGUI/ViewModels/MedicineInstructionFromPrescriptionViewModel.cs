using HealthCareCenter.Model;
using HealthCareCenter.Service;

namespace HealthCareCenter.PatientGUI.ViewModels
{
    internal class MedicineInstructionFromPrescriptionViewModel : ViewModelBase
    {
        Prescription _prescription;
        MedicineInstruction _medicineInstruction;

        public int PrescriptionID => _prescription.ID;
        public int DoctorID => _prescription.DoctorID;
        public string DoctorName => UserService.GetFullName(_prescription.DoctorID);
        public int InstructionID => _medicineInstruction.ID;
        public int MedicineID => _medicineInstruction.MedicineID;
        public string MedicineName => MedicineService.GetName(_medicineInstruction.MedicineID);

        public MedicineInstructionFromPrescriptionViewModel(Prescription prescription, MedicineInstruction medicineInstruction)
        {
            _prescription = prescription;
            _medicineInstruction = medicineInstruction;
        }
    }
}
