using HealthCareCenter.Controller;
using HealthCareCenter.Model;
using HealthCareCenter.Service;
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
    /// Interaction logic for ComplexHospitalRoomRenovationSplitWindow.xaml
    /// </summary>
    public partial class ComplexHospitalRoomRenovationSplitWindow : Window
    {
        private Manager _signedManager;
        private ComplexRoomRenovationSplitController _controller = new ComplexRoomRenovationSplitController();

        private string[] _header = {
            "Room1 ID", "Room1 Name", "Room1 Type",
            "Room2 ID", "Room2 Name", "Room2 Type",
            "Start Date", "Finish Date",
            "Split Room ID", "Split Room Name", "Split Room Type"
        };

        public ComplexHospitalRoomRenovationSplitWindow(Manager manager)
        {
            _signedManager = manager;
            InitializeComponent();
            FillAllComboBoxes();
            FillDataGridHospitalRooms();
            AddDataGridHeader(DataGridHospitalRoomsRenovationSplit, _header);
            FillDataGridHospitalRoomsRenovationSplit();
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

        private void FillAllComboBoxes()
        {
            Room1TypeComboBox.Items.Add(new ComboBoxItem() { Content = Enums.RoomType.Checkup });
            Room1TypeComboBox.Items.Add(new ComboBoxItem() { Content = Enums.RoomType.Operation });
            Room1TypeComboBox.Items.Add(new ComboBoxItem() { Content = Enums.RoomType.Rest });
            Room1TypeComboBox.Items.Add(new ComboBoxItem() { Content = Enums.RoomType.Other });
            Room1TypeComboBox.SelectedItem = Room1TypeComboBox.Items[0];

            Room2TypeComboBox.Items.Add(new ComboBoxItem() { Content = Enums.RoomType.Checkup });
            Room2TypeComboBox.Items.Add(new ComboBoxItem() { Content = Enums.RoomType.Operation });
            Room2TypeComboBox.Items.Add(new ComboBoxItem() { Content = Enums.RoomType.Rest });
            Room2TypeComboBox.Items.Add(new ComboBoxItem() { Content = Enums.RoomType.Other });
            Room2TypeComboBox.SelectedItem = Room2TypeComboBox.Items[0];
        }

        private void FillDataGridHospitalRooms()
        {
            DataGridHospitalRooms.Items.Clear();

            foreach (HospitalRoom room in _controller.GetRoomsForDisplay())
            {
                DataGridHospitalRooms.Items.Add(room);
            }
        }

        private void FillDataGridHospitalRoomsRenovationSplit()
        {
            DataGridHospitalRoomsRenovationSplit.Items.Clear();

            foreach (List<string> renovation in _controller.GetSplitRenovationsForDisplay())
            {
                AddDataGridRow(DataGridHospitalRoomsRenovationSplit, _header, renovation);
            }
        }

        private void SplitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string splitRoomId = RoomIDTextBox.Text;
                string room1Name = Room1NameTextBox.Text;
                string room2Name = Room2NameTextBox.Text;
                string room1Type = Room1TypeComboBox.Text;
                string room2Type = Room2TypeComboBox.Text;
                string startDate = StartDatePicker.Text;
                string finishDate = FinishDatePicker.Text;

                _controller.Split(splitRoomId, room1Name, room2Name, room1Type, room2Type, startDate, finishDate);

                HospitalRoom newRoom1 = _controller.GenerateNewRoom1(room1Type, room1Name);
                HospitalRoom newRoom2 = _controller.GenerateNewRoom2(room2Type, room2Name);
                HospitalRoom splitRoom = _controller.GetSplitRoom(splitRoomId);

                FillDataGridHospitalRooms();
                FillDataGridHospitalRoomsRenovationSplit();

                DateTime parsedFinishDate = Convert.ToDateTime(finishDate);
                ShowWindow(new EquipmentIrrevocableRearrangementWindow(_signedManager, parsedFinishDate, splitRoom, newRoom1, newRoom2));
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