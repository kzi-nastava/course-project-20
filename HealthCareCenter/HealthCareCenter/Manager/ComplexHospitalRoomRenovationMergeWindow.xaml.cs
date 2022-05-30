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

        private string[] header = {
            "Room1 ID", "Room1 Name", "Room1 Type",
            "Room2 ID", "Room2 Name", "Room2 Type",
            "New Room ID", "New Room Name", "New Room Type"
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

        private bool IsDateBeforeCurrentDate(DateTime date)
        {
            DateTime now = DateTime.Now;
            int value = DateTime.Compare(date, now);
            return value < 0;
        }

        private bool IsFinishDateBeforeStartDate(DateTime startDate, DateTime finishDate)
        {
            int value = DateTime.Compare(finishDate, startDate);
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
                    HospitalRoom room1 = HospitalRoomForRenovationService.Get(renovation.Room1ID);
                    HospitalRoom room2 = HospitalRoomForRenovationService.Get(renovation.Room2ID);
                    HospitalRoom newRoom = HospitalRoomUnderConstructionService.Get(renovation.MainRoomID);

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

        private bool IsHospitalRoomValide(string roomId)
        {
            if (!IsRoomIdInputValide(roomId))
            {
                MessageBox.Show($"Error, bad input for roomId={roomId}");
                return false;
            }
            int parsedHospitalRoomId = Convert.ToInt32(roomId);

            HospitalRoom room = HospitalRoomService.Get(parsedHospitalRoomId);

            if (!IsHospitalRoomFound(room))
            {
                MessageBox.Show($"Error, room with id={parsedHospitalRoomId} not found");
                return false;
            }

            return true;
        }

        private bool IsPossibleRenovation(HospitalRoom room)
        {
            if (room.ContainsAnyAppointment())
            {
                MessageBox.Show($"Error, room with id={room.ID} contains appointmnt!");
                return false;
            }

            if (room.ContaninsAnyRearrangement())
            {
                MessageBox.Show($"Error, room with id={room.ID} contains rearrangements!");
                return false;
            }

            return true;
        }

        private bool IsDateValide(string date)
        {
            if (!IsDateInputValide(date))
            {
                MessageBox.Show($"Error, bad input for date={date}");
                return false;
            }
            DateTime parsedDate = Convert.ToDateTime(date);

            if (IsDateBeforeCurrentDate(parsedDate))
            {
                MessageBox.Show($"Error, date={parsedDate} is before current date!");
                return false;
            }

            return true;
        }

        public ComplexHospitalRoomRenovationMergeWindow(Manager manager)
        {
            _signedManager = manager;
            InitializeComponent();
            FillDataGridHospitalRooms();
            AddDataGridHeader(DataGridHospitalRoomsRenovationMerge, header);
            FillDataGridHospitalRoomsRenovationMerge();
            FillNewRoomTypeComboBox();
        }

        private void MergeButton_Click(object sender, RoutedEventArgs e)
        {
            string room1Id = Room1IDTextBox.Text;
            string room2Id = Room2IDTextBox.Text;
            string renovationStartDate = StartDatePicker.Text;
            string renovationFinishDate = FinishDatePicker.Text;
            string newRoomName = NewRoomNameTextBox.Text;
            string newRoomType = NewRoomTypeComboBox.Text;

            if (!IsHospitalRoomValide(room1Id))
            {
                return;
            }
            if (!IsHospitalRoomValide(room2Id))
            {
                return;
            }
            if (room1Id == room2Id)
            {
                MessageBox.Show("Error, rooms have same id!");
                return;
            }
            int parsedRoom1Id = Convert.ToInt32(room1Id);
            int parsedRoom2Id = Convert.ToInt32(room2Id);
            HospitalRoom room1 = HospitalRoomService.Get(parsedRoom1Id);
            HospitalRoom room2 = HospitalRoomService.Get(parsedRoom2Id);

            if (!IsPossibleRenovation(room1))
            {
                return;
            }
            if (!IsPossibleRenovation(room2))
            {
                return;
            }

            if (!IsDateValide(renovationStartDate))
            {
                return;
            }
            if (!IsDateValide(renovationFinishDate))
            {
                return;
            }
            DateTime parsedRenovationStartDate = Convert.ToDateTime(renovationStartDate);
            DateTime parsedRenovationFinishDate = Convert.ToDateTime(renovationFinishDate);
            if (IsFinishDateBeforeStartDate(parsedRenovationStartDate, parsedRenovationFinishDate))
            {
                MessageBox.Show("Error, finish date is before start date");
                return;
            }

            Enum.TryParse(newRoomType, out Enums.RoomType parsedNewRoomType);
            if (!IsHospitalRoomNameInputValide(newRoomName))
            {
                MessageBox.Show("Error, bad input for name of new hospital room!");
                return;
            }
            HospitalRoom newRoom = new HospitalRoom(parsedNewRoomType, newRoomName);

            // SetMergeRenovation
            // -----------------------------------------
            RenovationSchedule mergeRenovation = new RenovationSchedule(parsedRenovationStartDate, parsedRenovationFinishDate, room1, room2, newRoom, Enums.RenovationType.Merge);
            mergeRenovation.ScheduleMergeRenovation(room1, room2, newRoom);
            // -----------------------------------------

            FillDataGridHospitalRooms();
            FillDataGridHospitalRoomsRenovationMerge();
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

        private void ReffusedMedicineClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new ChangeMedicineRequestWindow(_signedManager));
        }

        private void LogOffItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new LoginWindow());
        }
    }
}