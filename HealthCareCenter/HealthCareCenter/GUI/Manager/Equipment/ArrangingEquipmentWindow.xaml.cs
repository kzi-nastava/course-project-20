using HealthCareCenter.Core.Equipment.Controllers;
using HealthCareCenter.Core.Equipment.Repositories;
using HealthCareCenter.Core.Equipment.Services;
using HealthCareCenter.Core.Medicine.Services;
using HealthCareCenter.Core.HealthRecords;
using HealthCareCenter.Core.Medicine.Repositories;
using HealthCareCenter.Core.Notifications.Repositories;
using HealthCareCenter.Core.Notifications.Services;
using HealthCareCenter.Core.Rooms;
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
    /// Interaction logic for ArrangingEquipmentWindow.xaml
    /// </summary>
    public partial class ArrangingEquipmentWindow : Window
    {
        private string[] _headerDataGridEquipment = new string[]
        {
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

        private Manager _signedManager;
        private readonly ArrangingEquipmentController _controller;

        public ArrangingEquipmentWindow(Manager manager, IRoomService roomService, IHospitalRoomUnderConstructionService hospitalRoomUnderConstructionService, IHospitalRoomForRenovationService hospitalRoomForRenovationService, IRenovationScheduleService renovationScheduleService, IEquipmentRearrangementService equipmentRearrangementService, IDoctorSurveyRatingService doctorSurveyRatingService, IMedicineCreationRequestService medicineCreationRequestService)
        {
            _roomService = roomService;
            _hospitalRoomUnderConstructionService = hospitalRoomUnderConstructionService;
            _hospitalRoomForRenovationService = hospitalRoomForRenovationService;
            _renovationScheduleService = renovationScheduleService;
            _equipmentRearrangementService = equipmentRearrangementService;
            _doctorSurveyRatingService = doctorSurveyRatingService;
            _medicineCreationRequestService = medicineCreationRequestService;

            _signedManager = manager;
            _controller = new ArrangingEquipmentController(
                equipmentRearrangementService, 
                roomService,
                new EquipmentService(
                    new EquipmentRepository()));
            InitializeComponent();
            TimeComboBox.SelectedItem = TimeComboBox.Items[0];
            AddDataGridHeader(DataGridEquipments, _headerDataGridEquipment);
            FillDataGridEquipment();
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
            List<List<string>> equipmentsForDisplay = _controller.GetEquipmentsForDisplay();

            foreach (List<string> equipmentAttributesToDisplay in equipmentsForDisplay)
            {
                AddDataGridRow(DataGridEquipments, _headerDataGridEquipment, equipmentAttributesToDisplay);
            }
        }

        private void TransferButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string newRoomId = NewRoomIdTextBox.Text;
                string equipmentForRearrangementId = EquipmentIdTextBox.Text;
                string rearrangementDate = DatePicker.Text;
                string rearrangementTime = TimeComboBox.Text;

                _controller.SetRearrangement(newRoomId, equipmentForRearrangementId, rearrangementDate, rearrangementTime);
                DataGridEquipments.Items.Clear();
                FillDataGridEquipment();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string equipmentForRearrangementId = EquipmentIdTextBox.Text;
                _controller.UndoRearrangement(equipmentForRearrangementId);

                DataGridEquipments.Items.Clear();
                FillDataGridEquipment();
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
                new RoomService(
                        new StorageRepository(),
                        new EquipmentService(
                            new EquipmentRepository()),
                        new HospitalRoomUnderConstructionService(
                            new HospitalRoomUnderConstructionRepository()),
                        new HospitalRoomForRenovationService(
                            new HospitalRoomForRenovationRepository())),
                new HospitalRoomUnderConstructionService(
                    new HospitalRoomUnderConstructionRepository()),
                new HospitalRoomForRenovationService(
                    new HospitalRoomForRenovationRepository()),
                new RenovationScheduleService(
                    new RoomService(new StorageRepository(),
                        new EquipmentService(
                            new EquipmentRepository()),
                        new HospitalRoomUnderConstructionService(
                            new HospitalRoomUnderConstructionRepository()),
                        new HospitalRoomForRenovationService(
                            new HospitalRoomForRenovationRepository())),
                    new HospitalRoomUnderConstructionService(
                        new HospitalRoomUnderConstructionRepository()),
                    new HospitalRoomForRenovationService(
                        new HospitalRoomForRenovationRepository()),
                    new RenovationScheduleRepository()),
                new EquipmentRearrangementService(
                    new RoomService(
                        new StorageRepository(),
                        new EquipmentService(
                            new EquipmentRepository()),
                        new HospitalRoomUnderConstructionService(
                            new HospitalRoomUnderConstructionRepository()),
                        new HospitalRoomForRenovationService(
                            new HospitalRoomForRenovationRepository())),
                    new EquipmentService(
                        new EquipmentRepository()),
                    new HospitalRoomUnderConstructionService(
                        new HospitalRoomUnderConstructionRepository())),
                new DoctorSurveyRatingService(),
                new MedicineCreationRequestService(
                    new MedicineCreationRequestRepository())));
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
            ShowWindow(new MedicineCreationRequestWindow(
                _signedManager,
                new RoomService(
                        new StorageRepository(),
                        new EquipmentService(
                            new EquipmentRepository()),
                        new HospitalRoomUnderConstructionService(
                            new HospitalRoomUnderConstructionRepository()),
                        new HospitalRoomForRenovationService(
                            new HospitalRoomForRenovationRepository())), 
                _hospitalRoomUnderConstructionService, 
                _hospitalRoomForRenovationService, 
                _renovationScheduleService, 
                _equipmentRearrangementService, 
                _doctorSurveyRatingService, 
                _medicineCreationRequestService));
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
            ShowWindow(new HealthcareSurveysOverviewWindow(
                _signedManager, 
                _roomService, 
                _hospitalRoomUnderConstructionService, 
                _hospitalRoomForRenovationService, 
                _renovationScheduleService, 
                _equipmentRearrangementService, 
                _doctorSurveyRatingService, _medicineCreationRequestService));
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
                _doctorSurveyRatingService, _medicineCreationRequestService));
        }

        private void LogOffItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new LoginWindow());
        }
    }
}