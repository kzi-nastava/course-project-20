using HealthCareCenter.Core.Medicine.Models;
using HealthCareCenter.Core.Medicine.Services;
using HealthCareCenter.Core.Prescriptions;
using HealthCareCenter.Core.Users.Services;

namespace HealthCareCenter.GUI.Patient.SharedViewModels
{
    internal class MedicineInstructionFromPrescriptionViewModel : ViewModelBase
    {
        private readonly Prescription _prescription;
        private readonly MedicineInstruction _medicineInstruction;
        private readonly IMedicineService _medicineService;

        public int PrescriptionID => _prescription.ID;
        public int DoctorID => _prescription.DoctorID;
        public string DoctorName => UserService.GetFullName(_prescription.DoctorID);
        public int InstructionID => _medicineInstruction.ID;
        public int MedicineID => _medicineInstruction.MedicineID;
        public string MedicineName => _medicineService.GetName(_medicineInstruction.MedicineID);

        public MedicineInstructionFromPrescriptionViewModel(
            Prescription prescription, 
            MedicineInstruction medicineInstruction,
            IMedicineService medicineService)
        {
            _prescription = prescription;
            _medicineInstruction = medicineInstruction;
            _medicineService = medicineService;
        }
    }
}
