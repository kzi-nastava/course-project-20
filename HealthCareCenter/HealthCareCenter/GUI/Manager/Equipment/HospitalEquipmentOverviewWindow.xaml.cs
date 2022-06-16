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
using HealthCareCenter.Core;
using HealthCareCenter.Core.Equipment.Repositories;
using HealthCareCenter.Core.Equipment.Services;
using HealthCareCenter.Core.Medicine.Services;
using HealthCareCenter.Core.HealthRecords;
using HealthCareCenter.Core.Medicine.Repositories;
using HealthCareCenter.Core.Medicine.Services;
using HealthCareCenter.Core.Notifications.Repositories;
using HealthCareCenter.Core.Notifications.Services;
using HealthCareCenter.Core.Rooms;
using HealthCareCenter.Core.Rooms.Controllers;
using HealthCareCenter.Core.Rooms.Repositories;
using HealthCareCenter.Core.Rooms.Services;
using HealthCareCenter.Core.Surveys.Services;
using HealthCareCenter.Core.Users.Models;

namespace HealthCareCenter
{
    /// <summary>
    /// Interaction logic for HospitalEquipmentReviewWindow.xaml
    /// </summary>
    public partial class HospitalEquipmentReviewWindow : Window
    {
        private string[] _headerDataGridEquipment = new string[] {
            "Equipment Id", "Current Room Id", "Equipment Type",
            "Equipment Name", "Move Time", "New Room Id"
        };

        private readonly IRoomService _roomService;
        private readonly IHospitalRoomUnderConstructionService _hospitalRoomUnderConstructionService;

        private readonly IHospitalRoomForRenovationService _hospitalRoomForRenovationService;
        private readonly IRenovationScheduleService _renovationScheduleService;

        private readonly IEquipmentRearrangementService _equipmentRearrangementService;
        private readonly IDoctorSurveyRatingService _doctorSurveyRatingService;

        private readonly IMedicineCreationRequestService _medicineCreationRequestService;

        private readonly HospitalRoomOverviewController _contoller;
        private Manager _signedManager;

        public HospitalEquipmentReviewWindow(User user, IRoomService roomService, IHospitalRoomUnderConstructionService hospitalRoomUnderConstructionService, IHospitalRoomForRenovationService hospitalRoomForRenovationService, IRenovationScheduleService renovationScheduleService, IEquipmentRearrangementService equipmentRearrangementService, IDoctorSurveyRatingService doctorSurveyRatingService, IMedicineCreationRequestService medicineCreationRequestService)
        {
            _roomService = roomService;
            _hospitalRoomUnderConstructionService = hospitalRoomUnderConstructionService;
            _hospitalRoomForRenovationService = hospitalRoomForRenovationService;
            _renovationScheduleService = renovationScheduleService;
            _equipmentRearrangementService = equipmentRearrangementService;
            _doctorSurveyRatingService = doctorSurveyRatingService;
            _medicineCreationRequestService = medicineCreationRequestService;

            _signedManager = (Manager)user;
            _contoller = new HospitalRoomOverviewController(
                equipmentRearrangementService, 
                roomService,
                new EquipmentService(
                    new EquipmentRepository()),
                new StorageRepository());
            InitializeComponent();
            AddDataGridHeader(DataGridEquipments, _headerDataGridEquipment);
            FillDataGridEquipment();
            FillAllComboBoxes();
        }

        private void FillRoomTypeComboBox(ComboBox RoomTypeComboBox)
        {
            RoomTypeComboBox.Items.Add(new ComboBoxItem() { Content = "" });
            RoomTypeComboBox.Items.Add(new ComboBoxItem() { Content = RoomType.Storage });
            RoomTypeComboBox.Items.Add(new ComboBoxItem() { Content = RoomType.Checkup });
            RoomTypeComboBox.Items.Add(new ComboBoxItem() { Content = RoomType.Operation });
            RoomTypeComboBox.Items.Add(new ComboBoxItem() { Content = RoomType.Rest });
            RoomTypeComboBox.Items.Add(new ComboBoxItem() { Content = RoomType.Other });
            RoomTypeComboBox.SelectedItem = RoomTypeComboBox.Items[0];
        }

        private void FillEquipmentTypeComboBox(ComboBox EquipmentTypeComboBox)
        {
            EquipmentTypeComboBox.Items.Add(new ComboBoxItem() { Content = "" });
            EquipmentTypeComboBox.Items.Add(new ComboBoxItem() { Content = EquipmentType.ForCheckup });
            EquipmentTypeComboBox.Items.Add(new ComboBoxItem() { Content = EquipmentType.ForHallway });
            EquipmentTypeComboBox.Items.Add(new ComboBoxItem() { Content = EquipmentType.ForSurgery });
            EquipmentTypeComboBox.Items.Add(new ComboBoxItem() { Content = EquipmentType.Furniture });
            EquipmentTypeComboBox.SelectedItem = EquipmentTypeComboBox.Items[0];
        }

        private void FillEquipmentAmountComboBox(ComboBox EquipmentAmountComboBox)
        {
            EquipmentAmountComboBox.Items.Add(new ComboBoxItem() { Content = "" });
            EquipmentAmountComboBox.Items.Add(new ComboBoxItem() { Content = "Out of stock" });
            EquipmentAmountComboBox.Items.Add(new ComboBoxItem() { Content = "0-10" });
            EquipmentAmountComboBox.Items.Add(new ComboBoxItem() { Content = "10+" });
            EquipmentAmountComboBox.SelectedItem = EquipmentAmountComboBox.Items[0];
        }

        private void FillAllComboBoxes()
        {
            FillRoomTypeComboBox(RoomTypeComboBox);
            FillEquipmentTypeComboBox(EquipmentTypeComboBox);
            FillEquipmentAmountComboBox(EquipmentAmountComboBox);
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

        private void FillDataGridEquipment()
        {
            DataGridEquipments.Items.Clear();
            List<List<string>> displayList = _contoller.GetAllEquipmentsForDisplay();
            foreach (List<string> equipmentAttributesToDisplay in displayList)
            {
                AddDataGridRow(DataGridEquipments, _headerDataGridEquipment, equipmentAttributesToDisplay);
            }
        }

        private void ShowSearchResultButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataGridEquipments.Items.Clear();
                string searchContent = SearchEquipmentTextBox.Text;
                string amount = EquipmentAmountComboBox.Text;
                string equipmentType = EquipmentTypeComboBox.Text;
                string roomType = RoomTypeComboBox.Text;

                List<List<string>> roomsForDisplay = _contoller.GetFilteredEquipmentSearchResult(searchContent, amount, equipmentType, roomType);
                foreach (List<string> equipmentAttributesToDisplay in roomsForDisplay)
                {
                    AddDataGridRow(DataGridEquipments, _headerDataGridEquipment, equipmentAttributesToDisplay);
                }
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
                _roomService, 
                _hospitalRoomUnderConstructionService, 
                _hospitalRoomForRenovationService, 
                _renovationScheduleService, 
                _equipmentRearrangementService, 
                _doctorSurveyRatingService, 
                _medicineCreationRequestService));
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
            ShowWindow(new ChangedMedicineCreationRequestWindow(
                _signedManager, 
                _roomService, 
                _hospitalRoomUnderConstructionService, 
                _hospitalRoomForRenovationService, 
                _renovationScheduleService, 
                _equipmentRearrangementService, 
                _doctorSurveyRatingService, 
                _medicineCreationRequestService));
        }

        private void HealthcareSurveysClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new HealthcareSurveysOverviewWindow(_signedManager, _roomService, _hospitalRoomUnderConstructionService, _hospitalRoomForRenovationService, _renovationScheduleService, _equipmentRearrangementService, _doctorSurveyRatingService, _medicineCreationRequestService));
        }

        private void DoctorSurveysClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new DoctorSurveysOverviewWindow(
                _signedManager,
                _roomService, 
                _hospitalRoomUnderConstructionService, 
                _hospitalRoomForRenovationService, 
                _renovationScheduleService, 
                _equipmentRearrangementService, 
                _doctorSurveyRatingService, 
                _medicineCreationRequestService));
        }

        private void LogOffItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new LoginWindow());
        }
    }
}