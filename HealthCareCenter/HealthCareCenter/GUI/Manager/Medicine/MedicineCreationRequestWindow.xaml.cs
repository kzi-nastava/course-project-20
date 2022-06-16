using HealthCareCenter.Core.Equipment.Repositories;
using HealthCareCenter.Core.Equipment.Services;
using HealthCareCenter.Core.HealthRecords;
using HealthCareCenter.Core.Medicine.Controllers;
using HealthCareCenter.Core.Medicine.Repositories;
using HealthCareCenter.Core.Medicine.Services;
using HealthCareCenter.Core.Notifications.Repositories;
using HealthCareCenter.Core.Notifications.Services;
using HealthCareCenter.Core.Rooms.Repositories;
using HealthCareCenter.Core.Rooms.Services;
using HealthCareCenter.Core.Surveys.Services;
using HealthCareCenter.Core.Users.Models;
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
    public partial class MedicineCreationRequestWindow : Window
    {
        private string[] _headerOfIngredientsDataGrid = { "Ingredient Name" };
        private MedicineCreationRequestController _controller = new MedicineCreationRequestController();
        private Manager _signedManager;

        public MedicineCreationRequestWindow(Manager manager)
        {
            _signedManager = manager;
            InitializeComponent();
            AddDataGridHeader(DataGridIngredients, _headerOfIngredientsDataGrid);
        }

        private void ClearAllElements()
        {
            MedicineNameTextBox.Text = "";
            MedicineNameTextBox.Text = "";
            IngredientTextBox.Text = "";
            ManufacturerTextBox.Text = "";
            DataGridIngredients.Items.Clear();
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
            DataGridIngredients.Items.Clear();
            foreach (string ingredient in _controller.AddedIngrediens)
            {
                List<string> row = CreateRow(ingredient);
                AddDataGridRow(DataGridIngredients, _headerOfIngredientsDataGrid, row);
            }
        }

        private void AddIngredientButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string ingredient = IngredientTextBox.Text;
                _controller.AddIngredient(ingredient);

                List<string> row = CreateRow(ingredient);
                AddDataGridRow(DataGridIngredients, _headerOfIngredientsDataGrid, row);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RemoveIngredientButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string ingredient = IngredientTextBox.Text;
                _controller.RemoveIngredient(ingredient);
                FillDataGridIngredient();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CreateRequestButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string medicineName = MedicineNameTextBox.Text;
                string medicineManufacturer = ManufacturerTextBox.Text;

                _controller.Send(medicineName, medicineManufacturer);
                ClearAllElements();
                MessageBox.Show("You have successfully created a medicine creation request!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ShowWindow(Window window)
        {
            window.Show();
            Close();
        }

        private void CrudHospitalRoomMenuItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new CrudHospitalRoomWindow(
                _signedManager, 
                new NotificationService(
                    new NotificationRepository(),
                    new HealthRecordService(
                        new HealthRecordRepository()),
                    new MedicineInstructionService(
                        new MedicineInstructionRepository()),
                    new MedicineService(
                        new MedicineRepository())), 
                new EquipmentRearrangementService(new EquipmentService(new EquipmentRepository())),
                new RoomService(
                    new EquipmentRearrangementService(new EquipmentService(new EquipmentRepository())),
                        new StorageRepository(),
                        new EquipmentService(
                            new EquipmentRepository()))));
        }

        private void EquipmentReviewMenuItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new HospitalEquipmentReviewWindow(
                _signedManager, 
                new EquipmentRearrangementService(new EquipmentService(new EquipmentRepository())),
                new RoomService(
                    new EquipmentRearrangementService(new EquipmentService(new EquipmentRepository())),
                        new StorageRepository(),
                        new EquipmentService(
                            new EquipmentRepository()))));
        }

        private void ArrangingEquipmentItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new ArrangingEquipmentWindow(
                _signedManager,
                new EquipmentRearrangementService(new EquipmentService(new EquipmentRepository())),
                new RoomService(
                    new EquipmentRearrangementService(new EquipmentService(new EquipmentRepository())),
                        new StorageRepository(),
                        new EquipmentService(
                            new EquipmentRepository()))));
        }

        private void SimpleRenovationItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new HospitalRoomRenovationWindow(
                _signedManager,
                new RoomService(
                    new EquipmentRearrangementService(new EquipmentService(new EquipmentRepository())),
                        new StorageRepository(),
                        new EquipmentService(
                            new EquipmentRepository()))));
        }

        private void ComplexRenovationMergeItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new ComplexHospitalRoomRenovationMergeWindow(
                _signedManager,
                new RoomService(
                    new EquipmentRearrangementService(new EquipmentService(new EquipmentRepository())),
                        new StorageRepository(),
                        new EquipmentService(
                            new EquipmentRepository()))));
        }

        private void ComplexRenovationSplitItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new ComplexHospitalRoomRenovationSplitWindow(
                _signedManager,
                new RoomService(
                    new EquipmentRearrangementService(new EquipmentService(new EquipmentRepository())),
                        new StorageRepository(),
                        new EquipmentService(
                            new EquipmentRepository()))));
        }

        private void CreateMedicineClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new MedicineCreationRequestWindow(_signedManager));
        }

        private void ReffusedMedicineClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new ChangedMedicineCreationRequestWindow(_signedManager));
        }

        private void HealthcareSurveysClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new HealthcareSurveysOverviewWindow(_signedManager));
        }

        private void DoctorSurveysClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new DoctorSurveysOverviewWindow(_signedManager, new DoctorSurveyRatingService()));
        }

        private void LogOffItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new LoginWindow());
        }
    }
}