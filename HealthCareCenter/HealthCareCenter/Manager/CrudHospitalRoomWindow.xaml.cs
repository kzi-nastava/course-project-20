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
            if (room == null)
            {
                return false;
            }

            return true;
        }

        private void ShowWindow(Window window)
        {
            window.Show();
            Close();
        }

        private void FillComboBox()
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

        private void DeleteHospitalRoom(HospitalRoom delteteRoom)
        {
            HospitalRoomService.DeleteRoom(delteteRoom);
            FillDataGridHospitalRooms();
        }

        public CrudHospitalRoomWindow(Manager manager)
        {
            _signedManager = manager;
            InitializeComponent();
            FillComboBox();
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
            HospitalRoomService.AddRoom(room);
            DataGridHospitalRooms.Items.Add(room);
            HospitalRoomIdTextBox.Text = "";
            HospitalRoomNameTextBox.Text = "";
        }

        private void DeleteHospitalRoomButton_Click(object sender, RoutedEventArgs e)
        {
            string roomId = HospitalRoomIdTextBox.Text;
            if (!IsHospitalRoomIdInputValide(roomId))
            {
                MessageBox.Show("You must enter hospital room Id!");
                return;
            }

            int parsedRoomId = Convert.ToInt32(roomId);
            Room room = RoomService.GetRoom(parsedRoomId);

            if (room.IsStorage())
            {
                MessageBox.Show($"You have entered id of storage");
                return;
            }

            HospitalRoom hospitalRoom = (HospitalRoom)room;

            if (!IsHospitalRoomFound(hospitalRoom))
            {
                MessageBox.Show($"Hospital room with {parsedRoomId} Id it's not found!");
                return;
            }

            if (hospitalRoom.ContainAnyEquipment())
            {
                MessageBox.Show($"Hospital room with {parsedRoomId} Id contain equipment.");
                return;
            }

            if (hospitalRoom.ContainsAnyAppointment())
            {
                MessageBox.Show($"Hospital room with {parsedRoomId} Id contain apointments.");
                return;
            }

            if (hospitalRoom.IsCurrentlyRenovating())
            {
                MessageBox.Show($"Hospital room with {parsedRoomId} Id is renovating.");
                return;
            }

            HospitalRoomIdTextBox.Text = "";
            HospitalRoomNameTextBox.Text = "";
            DeleteHospitalRoom(hospitalRoom);
        }

        private void UpdateHospitalRoom_Click(object sender, RoutedEventArgs e)
        {
            Enums.RoomType newType = (Enums.RoomType)Enum.Parse(typeof(Enums.RoomType), RoomTypeComboBox.Text);
            string newRoomName = HospitalRoomNameTextBox.Text;
            string roomId = HospitalRoomIdTextBox.Text;

            if (!IsHospitalRoomIdInputValide(roomId))
            {
                MessageBox.Show("You must enter hospital room Id");
                return;
            }
            int parsedRoomId = Convert.ToInt32(roomId);

            if (!IsHospitalRoomNameInputValide(newRoomName))
            {
                MessageBox.Show("You must enter room name");
                return;
            }

            Room room = RoomService.GetRoom(parsedRoomId);
            if (room.IsStorage())
            {
                MessageBox.Show($"You have entered id of storage");
                return;
            }
            HospitalRoom hospitalRoom = (HospitalRoom)room;
            if (!IsHospitalRoomFound(hospitalRoom))
            {
                MessageBox.Show($"Hospital room with {parsedRoomId} id it's not found");
                return;
            }

            hospitalRoom.Name = newRoomName;
            hospitalRoom.Type = newType;
            RoomService.UpdateRoom(hospitalRoom);
            FillDataGridHospitalRooms();
            HospitalRoomIdTextBox.Text = "";
            HospitalRoomNameTextBox.Text = "";
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
    }
}