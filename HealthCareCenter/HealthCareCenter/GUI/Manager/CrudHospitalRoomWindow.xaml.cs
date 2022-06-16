using HealthCareCenter.Core;
using HealthCareCenter.Core.Equipment.Services;
using HealthCareCenter.Core.Medicine.Services;
using HealthCareCenter.Core.Notifications;
using HealthCareCenter.Core.Notifications.Repositories;
using HealthCareCenter.Core.Notifications.Services;
using HealthCareCenter.Core.Rooms;
using HealthCareCenter.Core.Rooms.Controllers;
using HealthCareCenter.Core.Rooms.Models;
using HealthCareCenter.Core.Rooms.Services;
using HealthCareCenter.Core.Surveys.Services;
using HealthCareCenter.Core.Users.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace HealthCareCenter
{
    public partial class CrudHospitalRoomWindow : Window
    {
        private Manager _signedManager;
        private CRUDHospitalRoomController _controler;
        private INotificationService _notificationService;

        private readonly IRoomService _roomService;
        private readonly IHospitalRoomUnderConstructionService _hospitalRoomUnderConstructionService;

        private readonly IHospitalRoomForRenovationService _hospitalRoomForRenovationService;
        private readonly IRenovationScheduleService _renovationScheduleService;

        private readonly IEquipmentRearrangementService _equipmentRearrangementService;
        private readonly IDoctorSurveyRatingService _doctorSurveyRatingService;

        private readonly IMedicineCreationRequestService _medicineCreationRequestService;

        public CrudHospitalRoomWindow(Manager manager, INotificationService notificationService, IRoomService roomService, IHospitalRoomUnderConstructionService hospitalRoomUnderConstructionService, IHospitalRoomForRenovationService hospitalRoomForRenovationService, IRenovationScheduleService renovationScheduleService, IEquipmentRearrangementService equipmentRearrangementService, IDoctorSurveyRatingService doctorSurveyRatingService, IMedicineCreationRequestService medicineCreationRequestService)
        {
            _notificationService = notificationService;
            _roomService = roomService;
            _hospitalRoomUnderConstructionService = hospitalRoomUnderConstructionService;
            _hospitalRoomForRenovationService = hospitalRoomForRenovationService;
            _renovationScheduleService = renovationScheduleService;
            _equipmentRearrangementService = equipmentRearrangementService;
            _doctorSurveyRatingService = doctorSurveyRatingService;
            _medicineCreationRequestService = medicineCreationRequestService;

            _signedManager = manager;

            _controler = new CRUDHospitalRoomController(_roomService, hospitalRoomForRenovationService);
            InitializeComponent();
            FillRoomTypeComboBox();
            FillDataGridHospitalRooms();
            DisplayNotifications();
        }

        private void SetAllTextBoxesToBeBlank()
        {
            HospitalRoomIdTextBox.Text = "";
            HospitalRoomNameTextBox.Text = "";
        }

        private void FillRoomTypeComboBox()
        {
            RoomTypeComboBox.Items.Add(new ComboBoxItem() { Content = RoomType.Checkup });
            RoomTypeComboBox.Items.Add(new ComboBoxItem() { Content = RoomType.Operation });
            RoomTypeComboBox.Items.Add(new ComboBoxItem() { Content = RoomType.Rest });
            RoomTypeComboBox.Items.Add(new ComboBoxItem() { Content = RoomType.Other });
            RoomTypeComboBox.SelectedItem = RoomTypeComboBox.Items[0];
        }

        private void FillDataGridHospitalRooms()
        {
            DataGridHospitalRooms.Items.Clear();
            List<HospitalRoom> roomsForDisplay = _controler.GetRoomsToDisplay();
            foreach (HospitalRoom room in roomsForDisplay)
            {
                DataGridHospitalRooms.Items.Add(room);
            }
        }

        private void UpdateInterfaceAfterDeleteOfHospitalRoom()
        {
            FillDataGridHospitalRooms();
            SetAllTextBoxesToBeBlank();
        }

        private void UpdateInterfaceAfterAddingHospitalRoom(HospitalRoom room)
        {
            room.ID--;
            DataGridHospitalRooms.Items.Add(room);
            SetAllTextBoxesToBeBlank();
        }

        private void UpdateInterfaceAfterUpdateOfHospitalRoom()
        {
            FillDataGridHospitalRooms();
            SetAllTextBoxesToBeBlank();
        }

        private void AddHospitalRoomButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RoomType type = (RoomType)Enum.Parse(typeof(RoomType), RoomTypeComboBox.Text);
                string roomName = HospitalRoomNameTextBox.Text;

                _controler.Create(type, roomName);
                HospitalRoom room = new HospitalRoom(type, roomName);
                UpdateInterfaceAfterAddingHospitalRoom(room);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteHospitalRoomButton_Click(object sender, RoutedEventArgs e)

        {
            try
            {
                string roomId = HospitalRoomIdTextBox.Text;
                _controler.Delete(roomId);
                UpdateInterfaceAfterDeleteOfHospitalRoom();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateHospitalRoomButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RoomType newType = (RoomType)Enum.Parse(typeof(RoomType), RoomTypeComboBox.Text);
                string newRoomName = HospitalRoomNameTextBox.Text;
                string roomId = HospitalRoomIdTextBox.Text;

                _controler.Update(newRoomName, newType, roomId);
                UpdateInterfaceAfterUpdateOfHospitalRoom();
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

        private void DisplayNotifications()
        {
            List<Notification> notifications = _notificationService.GetUnopened(_signedManager);
            if (notifications.Count == 0)
            {
                return;
            }
            MessageBox.Show("You have new notifications.");
            foreach (Notification notification in notifications)
            {
                MessageBox.Show(notification.Message);
            }
        }
    }
}