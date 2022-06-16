using HealthCareCenter.Core.Equipment.Controllers;
using HealthCareCenter.Core.Equipment.Models;
using HealthCareCenter.Core.Equipment.Repositories;
using HealthCareCenter.Core.Equipment.Services;
using HealthCareCenter.Core.Medicine.Services;
using HealthCareCenter.Core.Rooms;
using HealthCareCenter.Core.Rooms.Models;
using HealthCareCenter.Core.Rooms.Repositories;
using HealthCareCenter.Core.Rooms.Services;
using HealthCareCenter.Core.Surveys.Services;
using HealthCareCenter.Core.Users.Models;
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

namespace HealthCareCenter
{
    /// <summary>
    /// Interaction logic for EquipmentIrrevocableRearrangementWindow.xaml
    /// </summary>
    public partial class EquipmentIrrevocableRearrangementWindow : Window
    {
        private Manager _signedManager;
        private EquipmentIrrevocableRearrangementController _contoller;

        private readonly IRoomService _roomService;
        private readonly IHospitalRoomUnderConstructionService _hospitalRoomUnderConstructionService;

        private readonly IHospitalRoomForRenovationService _hospitalRoomForRenovationService;
        private readonly IRenovationScheduleService _renovationScheduleService;

        private readonly IEquipmentRearrangementService _equipmentRearrangementService;
        private readonly IDoctorSurveyRatingService _doctorSurveyRatingService;

        private readonly IMedicineCreationRequestService _medicineCreationRequestService;

        public EquipmentIrrevocableRearrangementWindow(Manager manager, DateTime finishDate, HospitalRoom splitRoom, HospitalRoom room1, HospitalRoom room2, IRoomService roomService, IHospitalRoomUnderConstructionService hospitalRoomUnderConstructionService, IHospitalRoomForRenovationService hospitalRoomForRenovationService, IRenovationScheduleService renovationScheduleService, IEquipmentRearrangementService equipmentRearrangementService, IDoctorSurveyRatingService doctorSurveyRatingService, IMedicineCreationRequestService medicineCreationRequestService)
        {
            _roomService = roomService;
            _hospitalRoomUnderConstructionService = hospitalRoomUnderConstructionService;
            _hospitalRoomForRenovationService = hospitalRoomForRenovationService;
            _renovationScheduleService = renovationScheduleService;
            _equipmentRearrangementService = equipmentRearrangementService;
            _doctorSurveyRatingService = doctorSurveyRatingService;
            _medicineCreationRequestService = medicineCreationRequestService;

            _signedManager = manager;
            List<Equipment> splitRoomEquipments = roomService.GetAllEquipment(splitRoom);
            _contoller = new EquipmentIrrevocableRearrangementController(splitRoomEquipments, splitRoom, room1, room2, finishDate);

            InitializeComponent();
            FillNewRoomComboBox();
            FillDataGridEquipment();
        }

        private void FillNewRoomComboBox()
        {
            NewRoomComboBox.Items.Add(new ComboBoxItem() { Content = _contoller.Room1.Name });
            NewRoomComboBox.Items.Add(new ComboBoxItem() { Content = _contoller.Room2.Name });
            NewRoomComboBox.SelectedItem = NewRoomComboBox.Items[0];
        }

        private void FillDataGridEquipment()
        {
            DataGridRoomEquipment.Items.Clear();
            foreach (Equipment equipment in _contoller.SplitRoomEquipments)
            {
                DataGridRoomEquipment.Items.Add(equipment);
            }
        }

        private void ShowWindow(Window window)
        {
            window.Show();
            Close();
        }

        private void TransferButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_contoller.IsSplitRoomContainsEquipment())
                {
                    string equipmentId = EquipmentIdTextBox.Text;
                    string newRoom = NewRoomComboBox.Text;
                    _contoller.Transfer(equipmentId, newRoom);
                }

                if (_contoller.IsSplitRoomContainsEquipment())
                {
                    MessageBox.Show("Rearrangement is done!");
                    ShowWindow(new ComplexHospitalRoomRenovationSplitWindow(_signedManager, _roomService, _hospitalRoomUnderConstructionService, _hospitalRoomForRenovationService, _renovationScheduleService, _equipmentRearrangementService, _doctorSurveyRatingService, _medicineCreationRequestService));
                }

                FillDataGridEquipment();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}