using HealthCareCenter.Core.Medicine.Models;
using HealthCareCenter.Core.Medicine.Services;
using HealthCareCenter.GUI.Patient.Profile.ViewModels;
using HealthCareCenter.GUI.Patient.SharedCommands;
using System;
using System.Windows;

namespace HealthCareCenter.GUI.Patient.Profile.Commands
{
    internal class ShowInstructionCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            if (_viewModel.ChosenInstruction == null)
            {
                _ = MessageBox.Show("No instruction chosen", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MedicineInstruction instruction = _medicineInstructionService.GetSingle(_viewModel.ChosenInstruction.InstructionID);

            _viewModel.MedicineInstructionInfo = _medicineInstructionService.GetInfo(instruction);
        }

        private readonly MyPrescriptionsViewModel _viewModel;
        private readonly IMedicineInstructionService _medicineInstructionService;

        public ShowInstructionCommand(
            MyPrescriptionsViewModel viewModel,
            IMedicineInstructionService medicineInstructionService)
        {
            _viewModel = viewModel;
            _medicineInstructionService = medicineInstructionService;
        }
    }
}
