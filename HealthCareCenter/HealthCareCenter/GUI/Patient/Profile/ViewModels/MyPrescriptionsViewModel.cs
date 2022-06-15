using HealthCareCenter.Core.Medicine.Repositories;
using HealthCareCenter.Core.Medicine.Services;
using HealthCareCenter.Core.Prescriptions;
using HealthCareCenter.GUI.Patient.Profile.Commands;
using HealthCareCenter.GUI.Patient.SharedViewModels;
using System.Collections.Generic;
using System.Windows.Input;

namespace HealthCareCenter.GUI.Patient.Profile.ViewModels
{
    internal class MyPrescriptionsViewModel : ViewModelBase
    {
        public Core.Patients.Patient Patient { get; }

        public readonly List<Prescription> PatientPrescriptions;

        private string _notificationReceiveTime;
        public string NotificationReceiveTime
        {
            get => _notificationReceiveTime;
            set
            {
                _notificationReceiveTime = value;
                OnPropertyChanged(nameof(NotificationReceiveTime));
            }
        }

        private string _searchKeyword;
        public string SearchKeyword
        {
            get => _searchKeyword;
            set
            {
                _searchKeyword = value;
                OnPropertyChanged(nameof(SearchKeyword));
            }
        }

        private List<MedicineInstructionFromPrescriptionViewModel> _instructions;
        public List<MedicineInstructionFromPrescriptionViewModel> Instructions
        {
            get => _instructions;
            set
            {
                _instructions = value;
                OnPropertyChanged(nameof(Instructions));
            }
        }

        private MedicineInstructionFromPrescriptionViewModel _chosenInstruction;
        public MedicineInstructionFromPrescriptionViewModel ChosenInstruction
        {
            get => _chosenInstruction;
            set
            {
                _chosenInstruction = value;
                OnPropertyChanged(nameof(ChosenInstruction));
            }
        }

        private string _medicineInstructionInfo;
        public string MedicineInstructionInfo
        {
            get => _medicineInstructionInfo;
            set
            {
                _medicineInstructionInfo = value;
                OnPropertyChanged(nameof(MedicineInstructionInfo));
            }
        }

        public ICommand SetNotificationTime { get; }
        public ICommand SearchInstruction { get; }
        public ICommand ShowInstruction { get; }

        public MyPrescriptionsViewModel(
            IMedicineInstructionService medicineInstructionService,
            BasePrescriptionService prescriptionService,
            Core.Patients.Patient patient,
            NavigationStore navigationStore)
        {
            Patient = patient;

            PatientPrescriptions = prescriptionService.GetPatientPrescriptions(patient.HealthRecordID);
            List<MedicineInstructionFromPrescriptionViewModel> instructions = new List<MedicineInstructionFromPrescriptionViewModel>();
            foreach (Prescription prescription in PatientPrescriptions)
            {
                foreach (int instructionID in prescription.MedicineInstructionIDs)
                {
                    instructions.Add(
                        new MedicineInstructionFromPrescriptionViewModel(
                            prescription, 
                            medicineInstructionService.GetSingle(instructionID),
                            new MedicineService(
                                new MedicineRepository()))); ;
                }
            }
            Instructions = instructions;

            SetNotificationTime = new SetNotificationTimeCommand(this);
            SearchInstruction = new SearchInstructionsCommand(
                this,
                new MedicineInstructionService(
                    new MedicineInstructionRepository()),
                new MedicineService(
                    new MedicineRepository()));
            ShowInstruction = new ShowInstructionCommand(
                this,
                new MedicineInstructionService(
                    new MedicineInstructionRepository()));
        }
    }
}
