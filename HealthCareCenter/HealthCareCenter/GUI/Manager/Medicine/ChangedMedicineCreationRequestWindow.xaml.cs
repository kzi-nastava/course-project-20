using HealthCareCenter.Core.Equipment.Services;
using HealthCareCenter.Core.Medicine.Controllers;
using HealthCareCenter.Core.Medicine.Repositories;
using HealthCareCenter.Core.Medicine.Services;
using HealthCareCenter.Core.Notifications.Repositories;
using HealthCareCenter.Core.Notifications.Services;
using HealthCareCenter.Core.Rooms;
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
    /// Interaction logic for ChangeMedicineRequestWindow.xaml
    /// </summary>
    public partial class ChangedMedicineCreationRequestWindow : Window
    {
        private Manager _signedManager;
        private string[] _headerOfChangedRequestsDataGrid = { "ID", "Medicine Name" };
        private string[] _headerOfIngredientsDataGrid = { "Ingredient Name" };
        private ChangedMedicineCreationRequestController _controller;

        private readonly IRoomService _roomService;
        private readonly IHospitalRoomUnderConstructionService _hospitalRoomUnderConstructionService;

        private readonly IHospitalRoomForRenovationService _hospitalRoomForRenovationService;
        private readonly IRenovationScheduleService _renovationScheduleService;

        private readonly IEquipmentRearrangementService _equipmentRearrangementService;
        private readonly IDoctorSurveyRatingService _doctorSurveyRatingService;

        private readonly IMedicineCreationRequestService _medicineCreationRequestService;

        public ChangedMedicineCreationRequestWindow(Manager manager, IRoomService roomService, IHospitalRoomUnderConstructionService hospitalRoomUnderConstructionService, IHospitalRoomForRenovationService hospitalRoomForRenovationService, IRenovationScheduleService renovationScheduleService, IEquipmentRearrangementService equipmentRearrangementService, IDoctorSurveyRatingService doctorSurveyRatingService, IMedicineCreationRequestService medicineCreationRequestService)
        {
            _roomService = roomService;
            _hospitalRoomUnderConstructionService = hospitalRoomUnderConstructionService;
            _hospitalRoomForRenovationService = hospitalRoomForRenovationService;
            _renovationScheduleService = renovationScheduleService;
            _equipmentRearrangementService = equipmentRearrangementService;
            _doctorSurveyRatingService = doctorSurveyRatingService;
            _medicineCreationRequestService = medicineCreationRequestService;

            _controller = new ChangedMedicineCreationRequestController(medicineCreationRequestService, new MedicineCreationRequestRepository());

            _signedManager = manager;
            InitializeComponent();
            FillDataGridChangedRequests();

            AddDataGridHeader(DataGridChangedRequests, _headerOfChangedRequestsDataGrid);
            AddDataGridHeader(DataGridIngredients, _headerOfIngredientsDataGrid);

            CommentTextBlock.IsEnabled = false;
            IngredientTextBox.IsEnabled = false;
            AddIngredientButton.IsEnabled = false;
            RemoveIngredientButton.IsEnabled = false;
            SendButton.IsEnabled = false;
            MedicineNameTextBox.IsEnabled = false;
            ManufacturerTextBox.IsEnabled = false;
        }

        private List<string> CreateRowForDataGridIngredients(string ingredient)
        {
            List<string> row = new List<string>();
            row.Add(ingredient);
            return row;
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

        private void FillDataGridChangedRequests()
        {
            DataGridChangedRequests.Items.Clear();
            foreach (List<string> requestAttributeForDisplay in _controller.GetRequestsForDisplay())
            {
                AddDataGridRow(DataGridChangedRequests, _headerOfChangedRequestsDataGrid, requestAttributeForDisplay);
            }
        }

        private void FillDataGridIngredients()
        {
            DataGridIngredients.Items.Clear();
            foreach (string ingredient in _controller.AddedIngrediens)
            {
                List<string> row = CreateRowForDataGridIngredients(ingredient);
                AddDataGridRow(DataGridIngredients, _headerOfIngredientsDataGrid, row);
            }
        }

        private void DisplayRequestContentInTextBoxes()
        {
            MedicineNameTextBox.Text = _controller.DisplayedRequest.Name;
            ManufacturerTextBox.Text = _controller.DisplayedRequest.Manufacturer;
            CommentTextBlock.Text = _controller.DisplayedRequest.DenyComment;
        }

        private void EnableComponentsAfterDispaly()
        {
            IngredientTextBox.IsEnabled = true;
            AddIngredientButton.IsEnabled = true;
            RemoveIngredientButton.IsEnabled = true;
            SendButton.IsEnabled = true;
            MedicineNameTextBox.IsEnabled = true;
            ManufacturerTextBox.IsEnabled = true;
        }

        private void DisableComponentsAfterSend()
        {
            IngredientTextBox.IsEnabled = false;
            AddIngredientButton.IsEnabled = false;
            RemoveIngredientButton.IsEnabled = false;
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
            DataGridIngredients.Items.Clear();
        }

        private void DisplayButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string changeRequistId = ChangeRequestIdTextBox.Text;
                _controller.DisplayRequest(changeRequistId);

                EnableComponentsAfterDispaly();
                FillDataGridIngredients();
                DisplayRequestContentInTextBoxes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddIngredientButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string ingredient = IngredientTextBox.Text;
                _controller.AddIngredient(ingredient);
                List<string> row = CreateRowForDataGridIngredients(ingredient);
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
                FillDataGridIngredients();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string medicineName = MedicineNameTextBox.Text;
                string manufacturer = ManufacturerTextBox.Text;
                _controller.Send(medicineName, manufacturer);
                ClearAllElements();
                DisableComponentsAfterSend();
                FillDataGridChangedRequests();

                MessageBox.Show("You have successfully send medicine on review!");
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
            ShowWindow(new CrudHospitalRoomWindow(_signedManager, new NotificationService(new NotificationRepository()), _roomService, _hospitalRoomUnderConstructionService, _hospitalRoomForRenovationService, _renovationScheduleService, _equipmentRearrangementService, _doctorSurveyRatingService, _medicineCreationRequestService));
        }

        private void EquipmentReviewMenuItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new HospitalEquipmentReviewWindow(_signedManager, _roomService, _hospitalRoomUnderConstructionService, _hospitalRoomForRenovationService, _renovationScheduleService, _equipmentRearrangementService, _doctorSurveyRatingService, _medicineCreationRequestService));
        }

        private void ArrangingEquipmentItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new ArrangingEquipmentWindow(_signedManager, _roomService, _hospitalRoomUnderConstructionService, _hospitalRoomForRenovationService, _renovationScheduleService, _equipmentRearrangementService, _doctorSurveyRatingService, _medicineCreationRequestService));
        }

        private void SimpleRenovationItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new HospitalRoomRenovationWindow(_signedManager, _roomService, _hospitalRoomUnderConstructionService, _hospitalRoomForRenovationService, _renovationScheduleService, _equipmentRearrangementService, _doctorSurveyRatingService, _medicineCreationRequestService));
        }

        private void ComplexRenovationMergeItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new ComplexHospitalRoomRenovationMergeWindow(_signedManager, _roomService, _hospitalRoomUnderConstructionService, _hospitalRoomForRenovationService, _renovationScheduleService, _equipmentRearrangementService, _doctorSurveyRatingService, _medicineCreationRequestService));
        }

        private void ComplexRenovationSplitItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new ComplexHospitalRoomRenovationSplitWindow(_signedManager, _roomService, _hospitalRoomUnderConstructionService, _hospitalRoomForRenovationService, _renovationScheduleService, _equipmentRearrangementService, _doctorSurveyRatingService, _medicineCreationRequestService));
        }

        private void CreateMedicineClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new MedicineCreationRequestWindow(_signedManager, _roomService, _hospitalRoomUnderConstructionService, _hospitalRoomForRenovationService, _renovationScheduleService, _equipmentRearrangementService, _doctorSurveyRatingService, _medicineCreationRequestService));
        }

        private void ReffusedMedicineClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new ChangedMedicineCreationRequestWindow(_signedManager, _roomService, _hospitalRoomUnderConstructionService, _hospitalRoomForRenovationService, _renovationScheduleService, _equipmentRearrangementService, _doctorSurveyRatingService, _medicineCreationRequestService));
        }

        private void HealthcareSurveysClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new HealthcareSurveysOverviewWindow(_signedManager, _roomService, _hospitalRoomUnderConstructionService, _hospitalRoomForRenovationService, _renovationScheduleService, _equipmentRearrangementService, _doctorSurveyRatingService, _medicineCreationRequestService));
        }

        private void DoctorSurveysClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new DoctorSurveysOverviewWindow(_signedManager, _roomService, _hospitalRoomUnderConstructionService, _hospitalRoomForRenovationService, _renovationScheduleService, _equipmentRearrangementService, _doctorSurveyRatingService, _medicineCreationRequestService));
        }

        private void LogOffItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new LoginWindow());
        }
    }
}