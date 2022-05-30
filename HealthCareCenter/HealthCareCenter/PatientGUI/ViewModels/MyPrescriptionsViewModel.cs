using HealthCareCenter.Model;
using HealthCareCenter.PatientGUI.Commands;
using HealthCareCenter.PatientGUI.Stores;
using HealthCareCenter.Service;
using System.Collections.Generic;
using System.Windows.Input;

namespace HealthCareCenter.PatientGUI.ViewModels
{
    internal class MyPrescriptionsViewModel : ViewModelBase
    {
        public Patient Patient { get; }

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

        public MyPrescriptionsViewModel(NavigationStore navigationStore, Patient patient)
        {
            Patient = patient;

            if (MedicineRepository.Medicines == null)
            {
                _ = MedicineRepository.Load();
            }
            if (PrescriptionRepository.Prescriptions == null)
            {
                _ = PrescriptionRepository.Load();
            }
            if (MedicineInstructionRepository.MedicineInstructions == null)
            {
                _ = MedicineInstructionRepository.Load();
            }

            PatientPrescriptions = PrescriptionService.GetPatientPrescriptions(patient.HealthRecordID);
            List<MedicineInstructionFromPrescriptionViewModel> instructions = new List<MedicineInstructionFromPrescriptionViewModel>();
            foreach (Prescription prescription in PatientPrescriptions)
            {
                foreach (int instructionID in prescription.MedicineInstructionIDs)
                {
                    instructions.Add(new MedicineInstructionFromPrescriptionViewModel(
                        prescription, MedicineInstructionService.GetSingle(instructionID))); ;
                }
            }
            Instructions = instructions;

            SetNotificationTime = new SetNotificationTimeCommand(this);
            SearchInstruction = new SearchInstructionsCommand(this);
            ShowInstruction = new ShowInstructionCommand(this);
        }
    }
}
