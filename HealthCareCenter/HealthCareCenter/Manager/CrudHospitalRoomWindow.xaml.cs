using HealthCareCenter.Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace HealthCareCenter
{
    public partial class CrudHospitalRoomWindow : Window
    {
        private Manager _signedUser;

        private bool IsHospitalRoomNameInputValide(string roomName)
        {
            if (roomName == "")
                return false;

            return true;
        }

        private bool IsHospitalRoomIdInputValide(ref int roomId)
        {
            if (Int32.TryParse(HospitalRoomIdTextBox.Text, out roomId))
                return true;
            return false;
        }

        private bool IsHospitalRoomFound(HospitalRoom room)
        {
            if (room == null)
                return false;

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
            List<HospitalRoom> rooms = HospitalRoomRepository.GetRooms();
            foreach (HospitalRoom room in rooms)
                DataGridHospitalRooms.Items.Add(room);
        }

        private void DeleteHospitalRoom(HospitalRoom delteteRoom)
        {
            HospitalRoomRepository.DeleteRoom(delteteRoom);
            FillDataGridHospitalRooms();
        }

        public CrudHospitalRoomWindow(User user)
        {
            _signedUser = (Manager)user;
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
            HospitalRoomRepository.AddRoom(room);
            DataGridHospitalRooms.Items.Add(room);
            HospitalRoomIdTextBox.Text = "";
            HospitalRoomNameTextBox.Text = "";
        }

        private void DeleteHospitalRoomButton_Click(object sender, RoutedEventArgs e)
        {
            int roomId = 0;
            if (!IsHospitalRoomIdInputValide(ref roomId))
            {
                MessageBox.Show("You must enter hospital room Id!");
                return;
            }

            HospitalRoom room = HospitalRoomRepository.GetRoomById(roomId);
            if (!IsHospitalRoomFound(room))
            {
                MessageBox.Show($"Hospital room with {roomId} Id it's not found!");
                return;
            }

            if (room.ContainAnyEquipment())
            {
                MessageBox.Show($"Hospital room with {roomId} Id contain equipment.");
                return;
            }

            if (room.ContainsAnyAppointment())
            {
                MessageBox.Show($"Hospital room with {roomId} Id contain apointments.");
                return;
            }

            HospitalRoomIdTextBox.Text = "";
            HospitalRoomNameTextBox.Text = "";
            DeleteHospitalRoom(room);
        }

        private void UpdateHospitalRoom_Click(object sender, RoutedEventArgs e)
        {
            Enums.RoomType newType = (Enums.RoomType)Enum.Parse(typeof(Enums.RoomType), RoomTypeComboBox.Text);
            string newRoomName = HospitalRoomNameTextBox.Text;
            int roomId = 0;

            if (!IsHospitalRoomIdInputValide(ref roomId))
            {
                MessageBox.Show("You must enter hospital room Id");
                return;
            }

            if (!IsHospitalRoomNameInputValide(newRoomName))
            {
                MessageBox.Show("You must enter room name");
                return;
            }

            HospitalRoom room = HospitalRoomRepository.GetRoomById(roomId);
            if (!IsHospitalRoomFound(room))
            {
                MessageBox.Show($"Hospital room with {roomId} id it's not found");
                return;
            }

            room.Name = newRoomName;
            room.Type = newType;
            HospitalRoomRepository.UpdateRoom(room);
            FillDataGridHospitalRooms();
            HospitalRoomIdTextBox.Text = "";
            HospitalRoomNameTextBox.Text = "";
        }

        private void CrudHospitalRoomMenuItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new CrudHospitalRoomWindow(_signedUser));
        }

        private void EquipmentReviewMenuItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new HospitalEquipmentReviewWindow(_signedUser));
        }

        private void ArrangingEquipmentItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new ArrangingEquipmentWindow(_signedUser));
        }

        private void LogOffItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new LoginWindow());
        }
    }
}