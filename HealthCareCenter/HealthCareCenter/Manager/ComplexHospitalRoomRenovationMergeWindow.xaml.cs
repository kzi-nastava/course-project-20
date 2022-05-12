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
    /// Interaction logic for ComplexHospitalRoomRenovationWindow.xaml
    /// </summary>
    public partial class ComplexHospitalRoomRenovationMergeWindow : Window
    {
        private Manager _signedManager;
        private string[] header = { "Room1 ID", "Room1 Name", "Room1 Type", "Room2 ID", "Room2 Name", "Room2 Type", "New Room ID", "New Room Name", "New Room Type" };

        public ComplexHospitalRoomRenovationMergeWindow(Manager manager)
        {
            _signedManager = manager;
            InitializeComponent();
            FillDataGridHospitalRooms();
            AddDataGridHeader(DataGridHospitalRoomsRenovationMerge, header);
            FillDataGridHospitalRoomsRenovationMerge();
            FillNewRoomTypeComboBox();
        }

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

        private void FillNewRoomTypeComboBox()
        {
            NewRoomTypeComboBox.Items.Add(new ComboBoxItem() { Content = Enums.RoomType.Checkup });
            NewRoomTypeComboBox.Items.Add(new ComboBoxItem() { Content = Enums.RoomType.Operation });
            NewRoomTypeComboBox.Items.Add(new ComboBoxItem() { Content = Enums.RoomType.Rest });
            NewRoomTypeComboBox.Items.Add(new ComboBoxItem() { Content = Enums.RoomType.Other });
            NewRoomTypeComboBox.SelectedItem = NewRoomTypeComboBox.Items[0];
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

        private void FillDataGridHospitalRoomsRenovationMerge()
        {
            DataGridHospitalRoomsRenovationMerge.Items.Clear();
            List<RenovationSchedule> renovations = RenovationScheduleService.GetRenovations();
            foreach (RenovationSchedule renovation in renovations)
            {
                if (renovation.RenovationType == Enums.RenovationType.Merge)
                {
                    HospitalRoom room1 = HospitalRoomForRenovationService.GetRoom(renovation.Room1ID);
                    HospitalRoom room2 = HospitalRoomForRenovationService.GetRoom(renovation.Room2ID);
                    HospitalRoom newRoom = HospitalRoomUnderConstructionService.GetRoom(renovation.MainRoomID);

                    List<string> row = new List<string> {
                    room1.ID.ToString(),room1.Name,room1.Type.ToString(),
                    room2.ID.ToString(),room2.Name,room2.Type.ToString(),
                    renovation.StartDate.ToString(),renovation.FinishDate.ToString(),
                    newRoom.ID.ToString(),newRoom.Name,newRoom.Type.ToString()
                };

                    AddDataGridRow(DataGridHospitalRoomsRenovationMerge, header, row);
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

        private void LogOffItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new LoginWindow());
        }

        private void MergeButton_Click(object sender, RoutedEventArgs e)
        {
            string room1Id = Room1IDTextBox.Text;
            string room2Id = Room2IDTextBox.Text;

            if (!IsRoomIdInputValide(room1Id))
            {
                MessageBox.Show($"Error, bad input for room1Id!");
                return;
            }

            if (!IsRoomIdInputValide(room2Id))
            {
                MessageBox.Show("Error, bad input for room2Id!");
                return;
            }

            if (room1Id == room2Id)
            {
                MessageBox.Show("Error, room1 Id and room2 Id must be different");
                return;
            }

            int parsedRoom1Id = Convert.ToInt32(room1Id);
            int parsedRoom2Id = Convert.ToInt32(room2Id);

            HospitalRoom room1 = HospitalRoomService.GetRoom(parsedRoom1Id);
            HospitalRoom room2 = HospitalRoomService.GetRoom(parsedRoom2Id);

            if (!IsHospitalRoomFound(room1))
            {
                MessageBox.Show($"Error, room with id={parsedRoom1Id} not found");
                return;
            }

            if (!IsHospitalRoomFound(room2))
            {
                MessageBox.Show($"Error, room with id={parsedRoom2Id} not found");
                return;
            }

            if (room1.ContainsAnyAppointment())
            {
                MessageBox.Show($"Error, room with id={parsedRoom1Id} contains appointmnt!");
                return;
            }

            if (room2.ContainsAnyAppointment())
            {
                MessageBox.Show($"Error, room with id={parsedRoom2Id} contains appointment!");
                return;
            }

            string startDate = StartDatePicker.Text;
            string finishDate = FinishDatePicker.Text;
            if (!IsDateInputValide(startDate))
            {
                MessageBox.Show("Error, bad input for start date");
                return;
            }

            if (!IsDateInputValide(finishDate))
            {
                MessageBox.Show("Error, bad input for finish date");
                return;
            }

            DateTime parsedStartDate = Convert.ToDateTime(startDate);
            DateTime parsedFinishDate = Convert.ToDateTime(finishDate);

            if (IsDateInputBeforeCurrentTime(parsedStartDate))
            {
                MessageBox.Show("Error, start date is before today");
                return;
            }

            if (IsDateInputBeforeCurrentTime(parsedFinishDate))
            {
                MessageBox.Show("Error, finish date is before today");
                return;
            }

            if (IsEndDateBeforeStartDate(parsedStartDate, parsedFinishDate))
            {
                MessageBox.Show("Error, finish date is before start date");
                return;
            }
            string newRoomName = NewRoomNameTextBox.Text;
            string newRoomType = NewRoomTypeComboBox.Text;

            Enum.TryParse(newRoomType, out Enums.RoomType parsedNewRoomType);

            if (!IsHospitalRoomNameInputValide(newRoomName))
            {
                MessageBox.Show("Error, bad input for name of new hospital room!");
                return;
            }

            if (room1.ContaninsAnyRearrangement())
            {
                MessageBox.Show("Error, room1 contains rearrangements!");
                return;
            }
            if (room2.ContaninsAnyRearrangement())
            {
                MessageBox.Show("Error, room2 contains rearrangements!");
                return;
            }

            HospitalRoom newRoom = new HospitalRoom(parsedNewRoomType, newRoomName);
            RenovationSchedule mergeRenovation = new RenovationSchedule(parsedStartDate, parsedFinishDate, room1, room2, newRoom, Enums.RenovationType.Merge);
            mergeRenovation.ScheduleMergeRenovation(room1, room2, newRoom);

            FillDataGridHospitalRooms();
            FillDataGridHospitalRoomsRenovationMerge();
        }
    }
}