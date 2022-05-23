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

        private string[] header = {
            "Room1 ID", "Room1 Name", "Room1 Type",
            "Room2 ID", "Room2 Name", "Room2 Type",
            "Start Date", "Finish Date",
            "Split Room ID", "Split Room Name", "Split Room Type"
        };

        private bool IsRoomIdInputValide(string roomId)
        {
            return Int32.TryParse(roomId, out int _);
        }

        private bool IsHospitalRoomFound(HospitalRoom room)
        {
            return room != null;
        }

        private bool IsHospitalRoomNameInputValide(string roomName)
        {
            return roomName != "";
        }

        private bool IsDateInputValide(string date)
        {
            return DateTime.TryParse(date, out DateTime _);
        }

        private bool IsDateInputBeforeCurrentTime(DateTime date)
        {
            DateTime now = DateTime.Now;
            int value = DateTime.Compare(date, now);
            return value < 0;
        }

        private bool IsEndDateBeforeStartDate(DateTime startDate, DateTime endDate)
        {
            int value = DateTime.Compare(endDate, startDate);
            return value < 0;
        }

        private bool IsDateValide(string startDate, string finishDate)
        {
            if (!IsDateInputValide(startDate))
            {
                MessageBox.Show("Error, bad input for start date");
                return false;
            }

            if (!IsDateInputValide(finishDate))
            {
                MessageBox.Show("Error, bad input for finish date");
                return false;
            }

            DateTime parsedStartDate = Convert.ToDateTime(startDate);
            DateTime parsedFinishDate = Convert.ToDateTime(finishDate);
            if (IsDateInputBeforeCurrentTime(parsedStartDate))
            {
                MessageBox.Show("Error, start date is before today");
                return false;
            }

            if (IsDateInputBeforeCurrentTime(parsedFinishDate))
            {
                MessageBox.Show("Error, finish date is before today");
                return false;
            }

            if (IsEndDateBeforeStartDate(parsedStartDate, parsedFinishDate))
            {
                MessageBox.Show("Error, finish date is before start date");
                return false;
            }

            return true;
        }

        private bool IsSplitRoomValide(string splitRoomId)
        {
            if (!IsRoomIdInputValide(splitRoomId))
            {
                MessageBox.Show($"Error, bad input for room id!");
                return false;
            }

            int parsedSplitRoomId = Convert.ToInt32(splitRoomId);
            HospitalRoom splitRoom = HospitalRoomService.Get(parsedSplitRoomId);

            if (!IsHospitalRoomFound(splitRoom))
            {
                MessageBox.Show($"Error, hospital room with id={parsedSplitRoomId} not found!");
                return false;
            }

            return true;
        }

        private bool IsPossibleRenovation(HospitalRoom splitRoom)
        {
            if (splitRoom.ContainsAnyAppointment())
            {
                MessageBox.Show($"Error, split room contains appointmnt!");
                return false;
            }

            if (splitRoom.ContaninsAnyRearrangement())
            {
                MessageBox.Show("Error, split room contains rearrngement");
                return false;
            }

            return true;
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

        public ComplexHospitalRoomRenovationSplitWindow(Manager manager)
        {
            _signedManager = manager;
            InitializeComponent();
            FillAllComboBoxes();
            FillDataGridHospitalRooms();
            AddDataGridHeader(DataGridHospitalRoomsRenovationSplit, header);
            FillDataGridHospitalRoomsRenovationSplit();
        }

        private void FillDataGridHospitalRooms()
        {
            DataGridHospitalRooms.Items.Clear();
            List<HospitalRoom> rooms = HospitalRoomService.GetRooms();
            foreach (HospitalRoom room in rooms)
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

        private void FillDataGridHospitalRoomsRenovationSplit()
        {
            DataGridHospitalRoomsRenovationSplit.Items.Clear();
            List<RenovationSchedule> renovations = RenovationScheduleService.GetRenovations();
            foreach (RenovationSchedule renovation in renovations)
            {
                if (renovation.RenovationType == Enums.RenovationType.Split)
                {
                    HospitalRoom room1 = HospitalRoomUnderConstructionService.Get(renovation.Room1ID);
                    HospitalRoom room2 = HospitalRoomUnderConstructionService.Get(renovation.Room2ID);
                    HospitalRoom splitRoom = HospitalRoomForRenovationService.Get(renovation.MainRoomID);

                    List<string> row = new List<string> {
                    room1.ID.ToString(),room1.Name,room1.Type.ToString(),
                    room2.ID.ToString(),room2.Name,room2.Type.ToString(),
                    renovation.StartDate.ToString(),renovation.FinishDate.ToString(),
                    splitRoom.ID.ToString(),splitRoom.Name,splitRoom.Type.ToString()
                };

                    AddDataGridRow(DataGridHospitalRoomsRenovationSplit, header, row);
                }
            }
        }

        private void ShowWindow(Window window)
        {
            window.Show();
            Close();
        }

        private void CrudHospitalRoomMenuItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new CrudHospitalRoomWindow(_signedManager));
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
            ShowWindow(new MedicineCreationWindow(_signedManager));
        }

        private void LogOffItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new LoginWindow());
        }

        private void SplitButton_Click(object sender, RoutedEventArgs e)
        {
            string splitRoomId = RoomIDTextBox.Text;

            string room1Name = Room1NameTextBox.Text;
            string room2Name = Room2NameTextBox.Text;

            string room1Type = Room1TypeComboBox.Text;
            string room2Type = Room2TypeComboBox.Text;
            Enum.TryParse(room1Type, out Enums.RoomType parsedRoom1Type);
            Enum.TryParse(room2Type, out Enums.RoomType parsedRoom2Type);

            string startDate = StartDatePicker.Text;
            string finishDate = FinishDatePicker.Text;

            if (!IsSplitRoomValide(splitRoomId))
            {
                return;
            }
            int parsedSplitRoomId = Convert.ToInt32(splitRoomId);
            HospitalRoom splitRoom = HospitalRoomService.Get(parsedSplitRoomId);
            if (!IsPossibleRenovation(splitRoom))
            {
                return;
            }

            if (!IsHospitalRoomNameInputValide(room1Name) || !IsHospitalRoomNameInputValide(room2Name))
            {
                MessageBox.Show("Error, bad input for room name!");
                return;
            }
            HospitalRoom newRoom1 = new HospitalRoom(parsedRoom1Type, room1Name);
            HospitalRoom newRoom2 = new HospitalRoom(parsedRoom2Type, room2Name);
            newRoom2.ID += 1;

            if (!IsDateValide(startDate, finishDate))
            {
                return;
            }
            DateTime parsedStartDate = Convert.ToDateTime(startDate);
            DateTime parsedFinishDate = Convert.ToDateTime(finishDate);

            // Set split renovation
            // ------------------------------
            RenovationSchedule splitRenovation = new RenovationSchedule(
                parsedStartDate, parsedFinishDate,
                newRoom1, newRoom2, splitRoom,
                Enums.RenovationType.Split);
            splitRenovation.ScheduleSplitRenovation(newRoom1, newRoom2, splitRoom);
            // ------------------------------

            FillDataGridHospitalRooms();
            FillDataGridHospitalRoomsRenovationSplit();

            ShowWindow(new EquipmentIrrevocableRearrangementWindow(_signedManager, parsedFinishDate, splitRoom, newRoom1, newRoom2));
        }
    }
}