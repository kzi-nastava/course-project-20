using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HealthCareCenter.Core.Rooms.Controllers;
using HealthCareCenter.Core.Users.Models;
using HealthCareCenter.Core.Rooms.Models;
using HealthCareCenter.Core.Notifications.Services;
using HealthCareCenter.Core.Notifications.Repositories;
using HealthCareCenter.Core.Equipment.Services;
using HealthCareCenter.Core.Rooms;
using HealthCareCenter.Core.Rooms.Services;
using HealthCareCenter.Core.Surveys.Services;
using HealthCareCenter.Core.Medicine.Services;

namespace HealthCareCenter
{
    /// <summary>
    /// Interaction logic for HospitalRoomRenovationWindow.xaml
    /// </summary>
    public partial class HospitalRoomRenovationWindow : Window
    {
        private Manager _signedManager;
        private HospitalRoomRenovaitonController _controller;

        private readonly IRoomService _roomService;
        private readonly IHospitalRoomUnderConstructionService _hospitalRoomUnderConstructionService;

        private readonly IHospitalRoomForRenovationService _hospitalRoomForRenovationService;
        private readonly IRenovationScheduleService _renovationScheduleService;

        private readonly IEquipmentRearrangementService _equipmentRearrangementService;
        private readonly IDoctorSurveyRatingService _doctorSurveyRatingService;

        private readonly IMedicineCreationRequestService _medicineCreationRequestService;

        public HospitalRoomRenovationWindow(Manager manager, IRoomService roomService, IHospitalRoomUnderConstructionService hospitalRoomUnderConstructionService, IHospitalRoomForRenovationService hospitalRoomForRenovationService, IRenovationScheduleService renovationScheduleService, IEquipmentRearrangementService equipmentRearrangementService, IDoctorSurveyRatingService doctorSurveyRatingService, IMedicineCreationRequestService medicineCreationRequestService)
        {
            _roomService = roomService;
            _hospitalRoomUnderConstructionService = hospitalRoomUnderConstructionService;
            _hospitalRoomForRenovationService = hospitalRoomForRenovationService;
            _renovationScheduleService = renovationScheduleService;
            _equipmentRearrangementService = equipmentRearrangementService;
            _doctorSurveyRatingService = doctorSurveyRatingService;
            _medicineCreationRequestService = medicineCreationRequestService;

            _signedManager = manager;
            _controller = new HospitalRoomRenovaitonController(roomService, renovationScheduleService);
            InitializeComponent();
            FillDataGridHospitalRooms();
            FillDataGridHospitalRoomsRenovation();
        }

        private void FillDataGridHospitalRooms()
        {
            DataGridHospitalRooms.Items.Clear();
            List<HospitalRoom> rooms = _controller.GetRoomsForDisplay();
            foreach (HospitalRoom room in rooms)
            {
                DataGridHospitalRooms.Items.Add(room);
            }
        }

        private void FillDataGridHospitalRoomsRenovation()
        {
            DataGridHospitalRoomsRenovation.Items.Clear();
            List<RenovationSchedule> renovations = _controller.GetRenovationsForDisplay();
            foreach (RenovationSchedule renovation in renovations)
            {
                DataGridHospitalRoomsRenovation.Items.Add(renovation);
            }
        }

        private void ScheduleRenovationButton_Click(object sender, RoutedEventArgs e)
        {
            string hospitalRoomForRenovationId = HospitalRoomIdTextBox.Text;
            string startDate = StartDatePicker.Text;
            string finishDate = EndDatePicker.Text;
            _controller.ScheduleRenovation(hospitalRoomForRenovationId, startDate, finishDate);
            FillDataGridHospitalRoomsRenovation();
            FillDataGridHospitalRooms();
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