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
using HealthCareCenter.Core.HealthRecords;
using HealthCareCenter.Core.Medicine.Services;
using HealthCareCenter.Core.Medicine.Repositories;

namespace HealthCareCenter
{
    /// <summary>
    /// Interaction logic for HospitalRoomRenovationWindow.xaml
    /// </summary>
    public partial class HospitalRoomRenovationWindow : Window
    {
        private Manager _signedManager;
        private HospitalRoomRenovaitonController _controller;

        public HospitalRoomRenovationWindow(Manager manager, IRoomService roomService)
        {
            _signedManager = manager;
            _controller = new HospitalRoomRenovaitonController(roomService);
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
            ShowWindow(new CrudHospitalRoomWindow(_signedManager,
                new NotificationService(
                    new NotificationRepository(),
                    new HealthRecordService(
                        new HealthRecordRepository()),
                    new MedicineInstructionService(
                        new MedicineInstructionRepository()),
                    new MedicineService(
                        new MedicineRepository())),
                new EquipmentRearrangementService(),
                new RoomService(new EquipmentRearrangementService())));
        }

        private void EquipmentReviewMenuItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new HospitalEquipmentReviewWindow(_signedManager, new EquipmentRearrangementService(), new RoomService(new EquipmentRearrangementService())));
        }

        private void ArrangingEquipmentItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new ArrangingEquipmentWindow(_signedManager, new EquipmentRearrangementService(), new RoomService(new EquipmentRearrangementService())));
        }

        private void SimpleRenovationItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new HospitalRoomRenovationWindow(_signedManager,
                new RoomService(new EquipmentRearrangementService())));
        }

        private void ComplexRenovationMergeItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new ComplexHospitalRoomRenovationMergeWindow(_signedManager, new RoomService(new EquipmentRearrangementService())));
        }

        private void ComplexRenovationSplitItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new ComplexHospitalRoomRenovationSplitWindow(_signedManager,
                new RoomService(new EquipmentRearrangementService())));
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