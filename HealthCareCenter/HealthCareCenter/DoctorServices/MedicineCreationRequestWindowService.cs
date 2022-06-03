using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using HealthCareCenter.Model;
using System.Data;
using HealthCareCenter.Enums;
using System.Globalization;
using System.ComponentModel;
using HealthCareCenter.DoctorGUI;
using HealthCareCenter.Service;

namespace HealthCareCenter.DoctorServices
{
    internal class MedicineCreationRequestWindowService
    {
        private DoctorWindow window;
        private User signedUser;
        public MedicineCreationRequestWindowService(DoctorWindow _window, User _signedUser)
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
