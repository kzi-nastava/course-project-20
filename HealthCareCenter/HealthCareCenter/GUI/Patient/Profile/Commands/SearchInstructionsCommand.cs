using HealthCareCenter.Core.Medicine.Models;
using HealthCareCenter.Core.Medicine.Services;
using HealthCareCenter.Core.Prescriptions;
using HealthCareCenter.GUI.Patient.Profile.ViewModels;
using HealthCareCenter.GUI.Patient.SharedCommands;
using HealthCareCenter.GUI.Patient.SharedViewModels;
using System.Collections.Generic;

namespace HealthCareCenter.GUI.Patient.Profile.Commands
{
    internal class SearchInstructionsCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            List<MedicineInstructionFromPrescriptionViewModel> instructions;
            if (string.IsNullOrEmpty(_viewModel.SearchKeyword))
            {
                // restarts the search and shows all patient's prescriptions

                instructions = new List<MedicineInstructionFromPrescriptionViewModel>();
                foreach (Prescription prescription in _viewModel.PatientPrescriptions)
                {
                    foreach (int instructionID in prescription.MedicineInstructionIDs)
                    {
                        instructions.Add(new MedicineInstructionFromPrescriptionViewModel(
                            prescription, MedicineInstructionService.GetSingle(instructionID))); ;
                    }
                }

                _viewModel.Instructions = instructions;
                return;
            }

            instructions = new List<MedicineInstructionFromPrescriptionViewModel>();
            foreach (Prescription prescription in _viewModel.PatientPrescriptions)
            {
                foreach (int instructionID in prescription.MedicineInstructionIDs)
                {
                    MedicineInstruction instruction = MedicineInstructionService.GetSingle(instructionID);
                    if (MedicineService.GetName(instruction.MedicineID).ToLower().Contains(
                        _viewModel.SearchKeyword.Trim().ToLower()))
                    {
                        instructions.Add(new MedicineInstructionFromPrescriptionViewModel(
                            prescription, MedicineInstructionService.GetSingle(instructionID)));
                    }
                }
            }

            _viewModel.Instructions = instructions;
        }

        private readonly MyPrescriptionsViewModel _viewModel;

        public SearchInstructionsCommand(MyPrescriptionsViewModel viewModel)
        {
            _viewModel = viewModel;
        }
    }
}
