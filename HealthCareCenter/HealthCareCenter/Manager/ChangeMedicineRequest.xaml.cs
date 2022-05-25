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
        private MedicineChangeRequest _displayedRequest;

        private bool IsValideIngredientName(string ingredient)
        {
            return ingredient.Trim() != "";
        }

        private bool IsIngridientAlreadyAdded(string ingredient)
        {
            return _displayedRequest.Medicine.Ingredients.Contains(ingredient);
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
            foreach (MedicineChangeRequest request in MedicineChangeRequestRepository.Requests)
            {
                List<string> row = new List<string>();
                row.Add(request.Medicine.ID.ToString());
                row.Add(request.Medicine.Name);
                AddDataGridRow(DataGridChangeRequests, _headerChangeRequests, row);
            }
        }

        private void FillDataGridIngredient(MedicineChangeRequest request)
        {
            Medicine medicineForChange = request.Medicine;
            DataGridIngredient.Items.Clear();

            List<string> row = new List<string>();
            foreach (string ingredient in medicineForChange.Ingredients)
            {
                row.Add(ingredient);
                AddDataGridRow(DataGridIngredient, _headerIngredient, row);
            }
        }

        private void FillAllFields(MedicineChangeRequest request)
        {
            Medicine medicineForChange = request.Medicine;
            string comment = request.Comment;

            MedicineNameTextBox.Text = medicineForChange.Name;
            CreationDatePicker.Text = medicineForChange.Creation.ToString();
            ExpirationDatePicker.Text = medicineForChange.Expiration.ToString();
            ManufacturerTextBox.Text = medicineForChange.Manufacturer;
            CommentTextBlock.Text = comment;
        }

        private void EnableComponentsAfterDispaly()
        {
            IngredientTextBox.IsEnabled = true;
            AddButton.IsEnabled = true;
            RemoveButton.IsEnabled = true;
        }

        private void DisableComponentsAfterSend()
        {
            IngredientTextBox.IsEnabled = false;
            AddButton.IsEnabled = false;
            RemoveButton.IsEnabled = false;
        }

        private void ClearAllElements()
        {
            ChangeRequestIdTextBox.Text = "";
            MedicineNameTextBox.Text = "";
            CreationDatePicker.Text = "";
            ExpirationDatePicker.Text = "";
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
        }

        private bool IsChangeRequestIdInputValide(string id)
        {
            return Int32.TryParse(id, out _);
        }

        private bool IsChangeRequestFound(string id)
        {
            int parsedId = Convert.ToInt32(id);
            MedicineChangeRequest request = MedicineChangeRequsetService.Get(parsedId);

            return request != null;
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
            if (!IsRequestValide(changeRequistId))
            {
                return;
            }
            int parsedChangeRequestId = Convert.ToInt32(changeRequistId);
            MedicineChangeRequest request = MedicineChangeRequsetService.Get(parsedChangeRequestId);

            _displayedRequest = request;

            EnableComponentsAfterDispaly();
            FillDataGridIngredient(_displayedRequest);
            FillAllFields(_displayedRequest);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string ingredient = IngredientTextBox.Text;
            if (!IsValideIngredientForAdding(ingredient))
            {
                return;
            }
            _displayedRequest.Medicine.Ingredients.Add(ingredient);
            List<string> row = new List<string>();
            row.Add(ingredient);
            AddDataGridRow(DataGridIngredient, _headerIngredient, row);
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            string ingredient = IngredientTextBox.Text;
            if (!IsValideIngredientForRemoving(ingredient))
            {
                return;
            }

            _displayedRequest.Medicine.Ingredients.Remove(ingredient);
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
            return _displayedRequest.Medicine.Ingredients.Count != 0;
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

        private bool IsDateInputValide(string date)
        {
            return DateTime.TryParse(date, out DateTime _);
        }

        private bool IsDateBeforeCurrentTime(DateTime date)
        {
            DateTime now = DateTime.Now;
            int value = DateTime.Compare(date, now);
            return value < 0;
        }

        private bool IsCreationDateBeforeExpirationDate(DateTime creationDate, DateTime expirationDate)
        {
            int value = DateTime.Compare(expirationDate, creationDate);
            return value < 0;
        }

        private bool IsDateValide(string creationDate, string expirationDate)
        {
            if (!IsDateInputValide(creationDate))
            {
                MessageBox.Show("Error, bad input for creation date!");
                return false;
            }
            DateTime parsedStartDate = Convert.ToDateTime(creationDate);

            if (!IsDateInputValide(expirationDate))
            {
                MessageBox.Show("Error, bad input for expiration date!");
                return false;
            }
            DateTime parsedFinishDate = Convert.ToDateTime(expirationDate);

            if (IsDateBeforeCurrentTime(parsedStartDate))
            {
                MessageBox.Show("Error, bad input for creation date!");
                return false;
            }

            if (IsDateBeforeCurrentTime(parsedFinishDate))
            {
                MessageBox.Show("Error, bad input for expiration date!");
                return false;
            }

            if (IsCreationDateBeforeExpirationDate(parsedStartDate, parsedFinishDate))
            {
                MessageBox.Show("Error, expiration date is before creation date!");
                return false;
            }

            return true;
        }

        private void SendCreationRequest(Medicine medicine)
        {
            MedicineCreationRequestService.Add(medicine);
            MedicineChangeRequsetService.Delete(medicine.ID);
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string medicineName = MedicineNameTextBox.Text;
            string creationDate = CreationDatePicker.Text;
            string expirationDate = ExpirationDatePicker.Text;
            string manufacturer = ManufacturerTextBox.Text;

            if (!IsValideMedicine(medicineName, manufacturer))
            {
                return;
            }
            if (!IsDateValide(creationDate, expirationDate))
            {
                return;
            }

            DateTime parsedCreationDate = Convert.ToDateTime(creationDate);
            DateTime parsedExpirationDate = Convert.ToDateTime(expirationDate);

            Medicine medicine = new Medicine(
                _displayedRequest.Medicine.ID, medicineName,
                parsedCreationDate, parsedExpirationDate,
                _displayedRequest.Medicine.Ingredients, manufacturer
                );
            SendCreationRequest(medicine);

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