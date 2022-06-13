using HealthCareCenter.Core;
using HealthCareCenter.Core.Notifications.Repositories;
using HealthCareCenter.Core.Notifications.Services;
using HealthCareCenter.Core.Rooms.Controllers;
using HealthCareCenter.Core.Rooms.Models;
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
    /// Interaction logic for ComplexHospitalRoomRenovationWindow.xaml
    /// </summary>
    public partial class ComplexHospitalRoomRenovationMergeWindow : Window
    {
        private string[] _header = {
            "Room1 ID", "Room1 Name", "Room1 Type",
            "Room2 ID", "Room2 Name", "Room2 Type",
            "New Room ID", "New Room Name", "New Room Type"
        };

        private Manager _signedManager;
        private ComplexHospitalRoomRenovationMergeController _controller = new ComplexHospitalRoomRenovationMergeController();

        public ComplexHospitalRoomRenovationMergeWindow(Manager manager)
        {
            _signedManager = manager;
            InitializeComponent();
            FillDataGridHospitalRooms();
            AddDataGridHeader(DataGridHospitalRoomsRenovationMerge, _header);
            FillDataGridHospitalRoomsRenovationMerge();
            FillNewRoomTypeComboBox();
        }

        private void FillNewRoomTypeComboBox()
        {
            NewRoomTypeComboBox.Items.Add(new ComboBoxItem() { Content = RoomType.Checkup });
            NewRoomTypeComboBox.Items.Add(new ComboBoxItem() { Content = RoomType.Operation });
            NewRoomTypeComboBox.Items.Add(new ComboBoxItem() { Content = RoomType.Rest });
            NewRoomTypeComboBox.Items.Add(new ComboBoxItem() { Content = RoomType.Other });
            NewRoomTypeComboBox.SelectedItem = NewRoomTypeComboBox.Items[0];
        }

        private void FillDataGridHospitalRooms()
        {
            DataGridHospitalRooms.Items.Clear();

            foreach (HospitalRoom room in _controller.GetRoomsForDisplay())
            {
                DataGridHospitalRooms.Items.Add(room);
            }
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

        private void FillDataGridHospitalRoomsRenovationMerge()
        {
            DataGridHospitalRoomsRenovationMerge.Items.Clear();

            foreach (List<string> renovation in _controller.GetAllMergeRenovations())
            {
                AddDataGridRow(DataGridHospitalRoomsRenovationMerge, _header, renovation);
            }
        }

        private void MergeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string room1Id = Room1IDTextBox.Text;
                string room2Id = Room2IDTextBox.Text;
                string renovationStartDate = StartDatePicker.Text;
                string renovationFinishDate = FinishDatePicker.Text;
                string newRoomName = NewRoomNameTextBox.Text;
                string newRoomType = NewRoomTypeComboBox.Text;

                _controller.Merge(room1Id, room2Id, renovationStartDate, renovationFinishDate, newRoomName, newRoomType);

                FillDataGridHospitalRooms();
                FillDataGridHospitalRoomsRenovationMerge();
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
            ShowWindow(new CrudHospitalRoomWindow(_signedManager, new NotificationService(new NotificationRepository())));
        }

        private void EquipmentReviewMenuItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new HospitalEquipmentReviewWindow(_signedManager));
        }

        private void ArrangingEquipmentItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new ArrangingEquipmentWindow(_signedManager));
        }

        private void SimpleRenovationItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new HospitalRoomRenovationWindow(_signedManager));
        }

        private void ComplexRenovationMergeItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new ComplexHospitalRoomRenovationMergeWindow(_signedManager));
        }

        private void ComplexRenovationSplitItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new ComplexHospitalRoomRenovationSplitWindow(_signedManager));
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
            ShowWindow(new DoctorSurveysOverviewWindow(_signedManager));
        }

        private void LogOffItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new LoginWindow());
        }
    }
}