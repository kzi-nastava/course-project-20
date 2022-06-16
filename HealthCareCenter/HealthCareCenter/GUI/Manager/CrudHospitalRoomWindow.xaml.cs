using HealthCareCenter.Core;
using HealthCareCenter.Core.Equipment.Repositories;
using HealthCareCenter.Core.Equipment.Services;
using HealthCareCenter.Core.Notifications;
using HealthCareCenter.Core.Notifications.Services;
using HealthCareCenter.Core.Rooms;
using HealthCareCenter.Core.Rooms.Controllers;
using HealthCareCenter.Core.Rooms.Models;
using HealthCareCenter.Core.Rooms.Repositories;
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
        private IRoomService _roomService;

        public CrudHospitalRoomWindow(Manager manager, INotificationService notificationService,
            IEquipmentRearrangementService equipmentRearrangementService, IRoomService roomService)
        {
            _signedManager = manager;
            _notificationService = notificationService;
            _roomService = roomService;
            _controler = new CRUDHospitalRoomController(_roomService);
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
            ShowWindow(new CrudHospitalRoomWindow(_signedManager, _notificationService,
                new EquipmentRearrangementService(new EquipmentService(new EquipmentRepository())),
                new RoomService(
                    new EquipmentRearrangementService(new EquipmentService(new EquipmentRepository())),
                    new StorageRepository(),
                    new EquipmentService(
                        new EquipmentRepository()))));
        }

        private void EquipmentReviewMenuItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new HospitalEquipmentReviewWindow(_signedManager,
                new EquipmentRearrangementService(new EquipmentService(new EquipmentRepository())),
                new RoomService(
                    new EquipmentRearrangementService(new EquipmentService(new EquipmentRepository())),
                    new StorageRepository(),
                    new EquipmentService(
                        new EquipmentRepository()))));
        }

        private void ArrangingEquipmentItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new ArrangingEquipmentWindow(_signedManager,
                new EquipmentRearrangementService(new EquipmentService(new EquipmentRepository())),
                new RoomService(
                    new EquipmentRearrangementService(new EquipmentService(new EquipmentRepository())),
                    new StorageRepository(),
                    new EquipmentService(
                        new EquipmentRepository()))));
        }

        private void SimpleRenovationItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new HospitalRoomRenovationWindow(_signedManager,
                new RoomService(
                    new EquipmentRearrangementService(new EquipmentService(new EquipmentRepository())),
                    new StorageRepository(),
                    new EquipmentService(
                        new EquipmentRepository()))));
        }

        private void ComplexRenovationMergeItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new ComplexHospitalRoomRenovationMergeWindow(_signedManager,
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