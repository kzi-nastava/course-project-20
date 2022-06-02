using HealthCareCenter.Model;
using HealthCareCenter.Service;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HealthCareCenter
{
    /// <summary>
    /// Interaction logic for ChangeMedicineRequestWindow.xaml
    /// </summary>
    public partial class ChangeMedicineRequestWindow : Window
    {
        private Manager _signedManager;
        private string[] _headerChangeRequests = { "ID", "Medicine Name" };
        private string[] _headerIngredient = { "Ingredient Name" };
        private MedicineCreationRequest _displayedRequest = null;

        private bool IsValideIngredientName(string ingredient)
        {
            return ingredient.Trim() != "";
        }

        private bool IsIngridientAlreadyAdded(string ingredient)
        {
            return _displayedRequest.Ingredients.Contains(ingredient);
        }

        private bool IsValideIngredientForAdding(string ingredient)
        {
            if (!IsValideIngredientName(ingredient))
            {
                MessageBox.Show("Invalide ingredient name");
                return false;
            }

            if (IsIngridientAlreadyAdded(ingredient))
            {
                MessageBox.Show("Ingredient is already added");
                return false;
            }

            return true;
        }

        private bool IsValideIngredientForRemoving(string ingredient)
        {
            if (!IsValideIngredientName(ingredient))
            {
                MessageBox.Show("Invalide ingredient name");
                return false;
            }

            if (!IsIngridientAlreadyAdded(ingredient))
            {
                MessageBox.Show("This ingredient doesn't exist");
                return false;
            }

            return true;
        }

        private void AddDataGridHeader(DataGrid dataGrid, string[] header)
        {
            foreach (string label in header)
            {
                DataGridTextColumn column = new DataGridTextColumn();
                column.Header = label;
                column.Binding = new Binding(label.Replace(' ', '_'));
                column.Width = 110;
                dataGrid.Columns.Add(column);
            }
        }

        private void AddDataGridRow(DataGrid dataGrid, string[] header, List<string> equipmentAttributesToDisplay)
        {
            dynamic row = new ExpandoObject();

            for (int i = 0; i < header.Length; i++)
            {
                ((IDictionary<String, Object>)row)[header[i].Replace(' ', '_')] = equipmentAttributesToDisplay[i];
            }

            dataGrid.Items.Add(row);
        }

        private void FillDataGridChangeRequests()
        {
            DataGridChangeRequests.Items.Clear();
            foreach (MedicineCreationRequest request in MedicineCreationRequestRepository.Requests)
            {
                if (request.State == Enums.RequestState.Denied)
                {
                    List<string> row = new List<string>();
                    row.Add(request.ID.ToString());
                    row.Add(request.Name);
                    AddDataGridRow(DataGridChangeRequests, _headerChangeRequests, row);
                }
            }
        }

        private void FillDataGridIngredient(MedicineCreationRequest request)
        {
            DataGridIngredient.Items.Clear();

            List<string> row = new List<string>();
            foreach (string ingredient in request.Ingredients)
            {
                row.Add(ingredient);
                AddDataGridRow(DataGridIngredient, _headerIngredient, row);
            }
        }

        private void FillAllFields(MedicineCreationRequest request)
        {
            MedicineNameTextBox.Text = request.Name;
            ManufacturerTextBox.Text = request.Manufacturer;
            CommentTextBlock.Text = request.DenyComment;
        }

        private void EnableComponentsAfterDispaly()
        {
            IngredientTextBox.IsEnabled = true;
            AddButton.IsEnabled = true;
            RemoveButton.IsEnabled = true;
            SendButton.IsEnabled = true;
            MedicineNameTextBox.IsEnabled = true;
            ManufacturerTextBox.IsEnabled = true;
        }

        private void DisableComponentsAfterSend()
        {
            IngredientTextBox.IsEnabled = false;
            AddButton.IsEnabled = false;
            RemoveButton.IsEnabled = false;
            SendButton.IsEnabled = false;
            MedicineNameTextBox.IsEnabled = false;
            ManufacturerTextBox.IsEnabled = false;
        }

        private void ClearAllElements()
        {
            ChangeRequestIdTextBox.Text = "";
            MedicineNameTextBox.Text = "";

            MedicineNameTextBox.Text = "";
            IngredientTextBox.Text = "";
            ManufacturerTextBox.Text = "";
            CommentTextBlock.Text = "";
            DataGridIngredient.Items.Clear();
            _displayedRequest = null;
        }

        public ChangeMedicineRequestWindow(Manager manager)
        {
            _signedManager = manager;
            InitializeComponent();
            FillDataGridChangeRequests();

            AddDataGridHeader(DataGridChangeRequests, _headerChangeRequests);
            AddDataGridHeader(DataGridIngredient, _headerIngredient);

            CommentTextBlock.IsEnabled = false;
            IngredientTextBox.IsEnabled = false;
            AddButton.IsEnabled = false;
            RemoveButton.IsEnabled = false;
            SendButton.IsEnabled = false;
            MedicineNameTextBox.IsEnabled = false;
            ManufacturerTextBox.IsEnabled = false;
        }

        private bool IsChangeRequestIdInputValide(string id)
        {
            return Int32.TryParse(id, out _);
        }

        private bool IsChangeRequestFound(string id)
        {
            int parsedId = Convert.ToInt32(id);
            MedicineCreationRequest request = MedicineCreationRequestService.Get(parsedId);

            if (request == null) { return false; }
            else if (request.State != Enums.RequestState.Denied) { return false; }

            return true;
        }

        private bool IsRequestValide(string id)
        {
            if (!IsChangeRequestIdInputValide(id))
            {
                MessageBox.Show("Error, bad input for change request id!");
                return false;
            }

            if (!IsChangeRequestFound(id))
            {
                MessageBox.Show($"Error, request with id={id} not found!");
                return false;
            }

            return true;
        }

        private void DisplayButton_Click(object sender, RoutedEventArgs e)
        {
            string changeRequistId = ChangeRequestIdTextBox.Text;
            if (!IsRequestValide(changeRequistId)) { return; }

            int parsedChangeRequestId = Convert.ToInt32(changeRequistId);
            MedicineCreationRequest request = MedicineCreationRequestService.Get(parsedChangeRequestId);

            _displayedRequest = request;

            EnableComponentsAfterDispaly();
            FillDataGridIngredient(_displayedRequest);
            FillAllFields(_displayedRequest);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string ingredient = IngredientTextBox.Text;
            if (!IsValideIngredientForAdding(ingredient)) { return; }

            _displayedRequest.Ingredients.Add(ingredient);

            List<string> row = new List<string>();
            row.Add(ingredient);

            AddDataGridRow(DataGridIngredient, _headerIngredient, row);
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            string ingredient = IngredientTextBox.Text;
            if (!IsValideIngredientForRemoving(ingredient)) { return; }

            _displayedRequest.Ingredients.Remove(ingredient);
            FillDataGridIngredient(_displayedRequest);
        }

        private bool IsValideMedicineName(string name)
        {
            return name.Trim() != "";
        }

        private bool IsValideMedicineManufacturer(string manufacturer)
        {
            return manufacturer.Trim() != "";
        }

        private bool DrugHasIngredient()
        {
            return _displayedRequest.Ingredients.Count != 0;
        }

        private bool IsValideMedicine(string name, string manfacturer)
        {
            if (!IsValideMedicineName(name))
            {
                MessageBox.Show("Bad input for medicine name");
                return false;
            }

            if (!IsValideMedicineManufacturer(manfacturer))
            {
                MessageBox.Show("Bad input for manufacturer");
                return false;
            }

            if (!DrugHasIngredient())
            {
                MessageBox.Show("Medicine must have ingredients");
                return false;
            }

            return true;
        }

        private bool IsPosisbleToSendRequest(string medicineName, string manufacturer)
        {
            if (!IsValideMedicine(medicineName, manufacturer)) { return false; }
            return true;
        }

        private void SendCreationRequest(MedicineCreationRequest request)
        {
            MedicineCreationRequestService.Delete(request);
            MedicineCreationRequestService.Add(request);
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string medicineName = MedicineNameTextBox.Text;
            string manufacturer = ManufacturerTextBox.Text;

            if (!IsPosisbleToSendRequest(medicineName, manufacturer)) { return; }

            MedicineCreationRequest medicineCreationRequest = new MedicineCreationRequest(
                _displayedRequest.ID, medicineName,
                _displayedRequest.Ingredients, manufacturer,
                Enums.RequestState.Waiting);
            SendCreationRequest(medicineCreationRequest);

            ClearAllElements();
            DisableComponentsAfterSend();
            FillDataGridChangeRequests();

            MessageBox.Show("You have successfully send medicine on review! ");
        }

        private void ShowWindow(Window window)
        {
            window.Show();
            Close();
        }

        private void CrudHospitalRoomMenuItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new CrudHospitalRoomWindow(_signedManager));
        }

        private void EquipmentReviewMenuItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new HospitalEquipmentReviewWindow(_signedManager));
        }

        private void ArrangingEquipmentItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new ArrangingEquipmentWindow(_signedManager));
        }

        private void SimpleRenovationItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new HospitalRoomRenovationWindow(_signedManager));
        }

        private void ComplexRenovationMergeItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new ComplexHospitalRoomRenovationMergeWindow(_signedManager));
        }

        private void ComplexRenovationSplitItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new ComplexHospitalRoomRenovationSplitWindow(_signedManager));
        }

        private void CreateMedicineClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new MedicineCreationWindow(_signedManager));
        }

        private void ReffusedMedicineClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new ChangeMedicineRequestWindow(_signedManager));
        }

        private void LogOffItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new LoginWindow());
        }
    }
}