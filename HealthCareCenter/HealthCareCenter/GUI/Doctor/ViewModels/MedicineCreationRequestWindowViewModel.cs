using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Globalization;
using System.ComponentModel;
using HealthCareCenter.DoctorGUI;
using HealthCareCenter.Core.Medicine.Services;
using HealthCareCenter.Core;
using HealthCareCenter.Core.Users.Models;
using HealthCareCenter.Core.Medicine.Models;
using HealthCareCenter.Core.Medicine.Repositories;

namespace HealthCareCenter.GUI.Doctor.ViewModels
{
    internal class MedicineCreationRequestWindowViewModel
    {
        private DoctorWindow window;
        private User signedUser;
        public MedicineCreationRequestWindowViewModel(DoctorWindow _window, User _signedUser)
        {
            window = _window;
            signedUser = _signedUser;
            FillMedicineRequestsTable();
        }

        public void FillMedicineRequestsTable()
        {
            window.medicineCreationRequestDataTable.Rows.Clear();
            foreach (MedicineCreationRequest request in MedicineCreationRequestRepository.Requests)
            {
                if (request.State != RequestState.Waiting)
                    continue;
                window.AddMedicineRequestToTable(request);
            }
            window.medicineCreationRequestDataGrid.ItemsSource = window.medicineCreationRequestDataTable.DefaultView;
        }
        public void AcceptRequestState()
        {
            int medicineCreationRequestID = TableService.GetRowItemID(window.medicineCreationRequestDataGrid, "Id");
            MedicineCreationRequest selectedRequest = MedicineCreationRequestService.GetMedicineCreationRequest(medicineCreationRequestID);
            selectedRequest.State = RequestState.Approved;
            FillMedicineRequestsTable();
        }

        public bool DenyRequestState()
        {
            string denyMessage = window.medicineRequestDeniedTextBox.Text;
            if (denyMessage == "")
            {
                MessageBox.Show("Please provide a reason");
                return false;
            }
            int medicineCreationRequestID = TableService.GetRowItemID(window.medicineCreationRequestDataGrid, "Id");
            if (medicineCreationRequestID == -1)
                return false;
            MedicineCreationRequest selectedRequest = MedicineCreationRequestService.GetMedicineCreationRequest(medicineCreationRequestID);
            selectedRequest.State = RequestState.Denied;
            selectedRequest.DenyComment = denyMessage;
            FillMedicineRequestsTable();
            return true;
        }


        public void ParseIngredients()
        {
            int medicineCreationRequestID = TableService.GetRowItemID(window.medicineCreationRequestDataGrid, "Id");
            MedicineCreationRequest request = MedicineCreationRequestService.GetMedicineCreationRequest(medicineCreationRequestID);
            string ingredients = MedicineCreationRequestService.GetIngredients(request);
            window.ingredientsTextBlock.Text = ingredients;
        }
    }
}
