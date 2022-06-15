using HealthCareCenter.Core.Equipment.Controllers;
using HealthCareCenter.Core.Equipment.Services;
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
    /// Interaction logic for ArrangingEquipmentWindow.xaml
    /// </summary>
    public partial class ArrangingEquipmentWindow : Window
    {
        private string[] _headerDataGridEquipment = new string[]
        {
            "Equipment Id", "Current Room Id", "Equipment Type",
            "Equipment Name", "Move Time", "New Room Id"
        };

        private Manager _signedManager;
        private readonly ArrangingEquipmentController _controller;

        public ArrangingEquipmentWindow(Manager manager, IEquipmentRearrangementService equipmentRearrangementService, IRoomService roomService)
        {
            _signedManager = manager;
            _controller = new ArrangingEquipmentController(equipmentRearrangementService, roomService);
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
            ShowWindow(new CrudHospitalRoomWindow(_signedManager,
                new NotificationService(new NotificationRepository()),
                new EquipmentRearrangementService(),
                new RoomService(new EquipmentRearrangementService())));
        }

        private void EquipmentReviewMenuItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new HospitalEquipmentReviewWindow(_signedManager, new EquipmentRearrangementService(), new RoomService(new EquipmentRearrangementService())));
        }

        private void ArrangingEquipmentItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new ArrangingEquipmentWindow(_signedManager,
                new EquipmentRearrangementService(),
                new RoomService(new EquipmentRearrangementService())));
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