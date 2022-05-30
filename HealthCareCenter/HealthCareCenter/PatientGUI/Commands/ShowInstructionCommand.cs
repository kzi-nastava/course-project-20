using HealthCareCenter.Model;
using HealthCareCenter.PatientGUI.Models;
using HealthCareCenter.PatientGUI.ViewModels;
using HealthCareCenter.Service;
using System;
using System.Windows;

namespace HealthCareCenter.PatientGUI.Commands
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

            PatientFunctionality patFunc = PatientFunctionality.GetInstance();
            MedicineInstruction instruction = MedicineInstructionService.GetSingle(Convert.ToInt32(_viewModel.ChosenInstruction.InstructionID));

            _viewModel.MedicineInstructionInfo = patFunc.GetMedicineInstructionInfo(instruction);
        }

        private readonly MyPrescriptionsViewModel _viewModel;

        public ShowInstructionCommand(MyPrescriptionsViewModel viewModel)
        {
            _viewModel = viewModel;
        }
    }
}
