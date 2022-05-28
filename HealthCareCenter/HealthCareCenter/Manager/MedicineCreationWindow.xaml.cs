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
    /// Interaction logic for MedicineCreationWindow.xaml
    /// </summary>
    public partial class MedicineCreationWindow : Window
    {
        private List<string> _ingredients = new List<string>();
        private string[] _header = { "Ingredient Name" };
        private Manager _signedManager;

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
            return _ingredients.Count != 0;
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

        private bool IsValideIngredientName(string ingredient)
        {
            return ingredient.Trim() != "";
        }

        private bool IsIngridientAlreadyAdded(string ingredient)
        {
            return _ingredients.Contains(ingredient);
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

        private void ClearAllElements()
        {
            MedicineNameTextBox.Text = "";
            CreationDatePicker.Text = "";
            ExpirationDatePicker.Text = "";
            MedicineNameTextBox.Text = "";
            IngredientTextBox.Text = "";
            ManufacturerTextBox.Text = "";
            DataGridIngredient.Items.Clear();
        }

        public MedicineCreationWindow(Manager manager)
        {
            _signedManager = manager;
            InitializeComponent();
            AddDataGridHeader(DataGridIngredient, _header);
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

        private List<string> CreateRow(string ingredient)
        {
            List<string> row = new List<string>();
            row.Add(ingredient);
            return row;
        }

        private void FillDataGridIngredient()
        {
            DataGridIngredient.Items.Clear();
            foreach (string ingredient in _ingredients)
            {
                List<string> row = CreateRow(ingredient);
                AddDataGridRow(DataGridIngredient, _header, row);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string ingredient = IngredientTextBox.Text;
            if (!IsValideIngredientForAdding(ingredient))
            {
                return;
            }
            _ingredients.Add(ingredient);
            List<string> row = CreateRow(ingredient);
            AddDataGridRow(DataGridIngredient, _header, row);
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            string ingredient = IngredientTextBox.Text;
            if (!IsValideIngredientForRemoving(ingredient))
            {
                return;
            }

            _ingredients.Remove(ingredient);
            FillDataGridIngredient();
        }

        private int GenerateMedicineId()
        {
            if (MedicineCreationRequestService.GetLargestId() > MedicineChangeRequsetService.GetLargestId())
            {
                return MedicineCreationRequestService.GetLargestId() + 1;
            }
            else
            {
                return MedicineChangeRequsetService.GetLargestId() + 1;
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            string medicineName = MedicineNameTextBox.Text;
            string medicineManufacturer = ManufacturerTextBox.Text;
            string creationDate = CreationDatePicker.Text;
            string expirationDate = ExpirationDatePicker.Text;
            if (!IsValideMedicine(medicineName, medicineManufacturer))
            {
                return;
            }
            if (!IsDateValide(creationDate, expirationDate))
            {
                return;
            }

            DateTime parsedCreationDate = Convert.ToDateTime(creationDate);
            DateTime parsedExpirationDate = Convert.ToDateTime(expirationDate);

            Medicine medicine = new Medicine(GenerateMedicineId(), medicineName, parsedCreationDate, parsedExpirationDate, _ingredients, medicineManufacturer);

            MedicineCreationRequestService.Add(medicine);
            ClearAllElements();
            MessageBox.Show("You have successfully created a medicine creation request!");
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