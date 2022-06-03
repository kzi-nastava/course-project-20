using HealthCareCenter.Model;
using HealthCareCenter.Service;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace HealthCareCenter
{
    public partial class CrudHospitalRoomWindow : Window
    {
        private Manager _signedManager;

        private bool IsHospitalRoomNameInputValide(string roomName)
        {
            return roomName != "";
        }

        private bool IsHospitalRoomIdInputValide(string roomId)
        {
            return Int32.TryParse(roomId, out int _);
        }

        private bool IsHospitalRoomFound(HospitalRoom room)
        {
            return room != null;
        }

        private bool IsPossibleToDeleteHospitalRoom(string roomId)
        {
            if (!IsHospitalRoomIdInputValide(roomId))
            {
                MessageBox.Show("You must enter hospital room Id!");
                return false;
            }

            int parsedRoomId = Convert.ToInt32(roomId);
            Room room = RoomService.Get(parsedRoomId);

            if (RoomService.IsStorage(room))
            {
                MessageBox.Show($"You have entered id of storage");
                return false;
            }
            HospitalRoom hospitalRoom = (HospitalRoom)room;
            if (!IsHospitalRoomFound(hospitalRoom))
            {
                MessageBox.Show($"Hospital room with {hospitalRoom.ID} Id it's not found!");
                return false;
            }

            if (RoomService.ContainAnyEquipment(hospitalRoom))
            {
                MessageBox.Show($"Hospital room with {hospitalRoom.ID} Id contain equipment.");
                return false;
            }

            if (HospitalRoomService.ContainsAnyAppointment(hospitalRoom))
            {
                MessageBox.Show($"Hospital room with {hospitalRoom.ID} Id contain apointments.");
                return false;
            }

            if (HospitalRoomService.IsCurrentlyRenovating(hospitalRoom))
            {
                MessageBox.Show($"Hospital room with {hospitalRoom.ID} Id is renovating.");
                return false;
            }

            return true;
        }

        private bool IsPossibleRoomToUpdate(string newRoomName, string roomId)
        {
            if (!IsHospitalRoomIdInputValide(roomId))
            {
                MessageBox.Show("Bad input for hospital room Id");
                return false;
            }
            int parsedRoomId = Convert.ToInt32(roomId);

            if (!IsHospitalRoomNameInputValide(newRoomName))
            {
                MessageBox.Show("You must enter room name");
                return false;
            }

            Room room = RoomService.Get(parsedRoomId);
            if (RoomService.IsStorage(room))
            {
                MessageBox.Show($"You have entered id of storage");
                return false;
            }
            HospitalRoom hospitalRoom = (HospitalRoom)room;
            if (!IsHospitalRoomFound(hospitalRoom))
            {
                MessageBox.Show($"Hospital room with {parsedRoomId} id it's not found");
                return false;
            }
            return true;
        }

        private void SetAllTextBoxesToBeBlank()
        {
            HospitalRoomIdTextBox.Text = "";
            HospitalRoomNameTextBox.Text = "";
        }

        private void FillRoomTypeComboBox()
        {
            RoomTypeComboBox.Items.Add(new ComboBoxItem() { Content = Enums.RoomType.Checkup });
            RoomTypeComboBox.Items.Add(new ComboBoxItem() { Content = Enums.RoomType.Operation });
            RoomTypeComboBox.Items.Add(new ComboBoxItem() { Content = Enums.RoomType.Rest });
            RoomTypeComboBox.Items.Add(new ComboBoxItem() { Content = Enums.RoomType.Other });
            RoomTypeComboBox.SelectedItem = RoomTypeComboBox.Items[0];
        }

        private void FillDataGridHospitalRooms()
        {
            DataGridHospitalRooms.Items.Clear();
            List<HospitalRoom> rooms = HospitalRoomService.GetRooms();
            foreach (HospitalRoom room in rooms)
            {
                DataGridHospitalRooms.Items.Add(room);
            }

            rooms = HospitalRoomForRenovationService.GetRooms();
            foreach (HospitalRoom room in rooms)
            {
                DataGridHospitalRooms.Items.Add(room);
            }
        }

        private void DeleteHospitalRoom(HospitalRoom room)
        {
            HospitalRoomService.Delete(room);
            FillDataGridHospitalRooms();
            SetAllTextBoxesToBeBlank();
        }

        private void CreateNewHospitalRoom(HospitalRoom room)
        {
            HospitalRoomService.Add(room);
            DataGridHospitalRooms.Items.Add(room);
            SetAllTextBoxesToBeBlank();
        }

        private void UpdateHospitalRoom(HospitalRoom room, string newRoomName, Enums.RoomType newType)
        {
            room.Name = newRoomName;
            room.Type = newType;
            RoomService.Update(room);
            FillDataGridHospitalRooms();
            SetAllTextBoxesToBeBlank();
        }

        public CrudHospitalRoomWindow(Manager manager)
        {
            _signedManager = manager;
            InitializeComponent();
            FillRoomTypeComboBox();
            FillDataGridHospitalRooms();
        }

        private void AddHospitalRoomButton_Click(object sender, RoutedEventArgs e)
        {
            Enums.RoomType type = (Enums.RoomType)Enum.Parse(typeof(Enums.RoomType), RoomTypeComboBox.Text);
            string roomName = HospitalRoomNameTextBox.Text;

            if (!IsHospitalRoomNameInputValide(roomName))
            {
                MessageBox.Show("You must enter room name");
                return;
            }

            HospitalRoom room = new HospitalRoom(type, roomName);
            CreateNewHospitalRoom(room);
        }

        private void DeleteHospitalRoomButton_Click(object sender, RoutedEventArgs e)

        {
            string roomId = HospitalRoomIdTextBox.Text;
            if (!IsPossibleToDeleteHospitalRoom(roomId)) { return; }

            int parsedRoomId = Convert.ToInt32(roomId);
            HospitalRoom hospitalRoom = HospitalRoomService.Get(parsedRoomId);

            DeleteHospitalRoom(hospitalRoom);
        }

        private void UpdateHospitalRoomButton_Click(object sender, RoutedEventArgs e)
        {
            Enums.RoomType newType = (Enums.RoomType)Enum.Parse(typeof(Enums.RoomType), RoomTypeComboBox.Text);
            string newRoomName = HospitalRoomNameTextBox.Text;
            string roomId = HospitalRoomIdTextBox.Text;
            if (!IsPossibleRoomToUpdate(newRoomName, roomId)) { return; }

            int parsedRoomId = Convert.ToInt32(roomId);
            HospitalRoom hospitalRoom = HospitalRoomService.Get(parsedRoomId);
            UpdateHospitalRoom(hospitalRoom, newRoomName, newType);
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