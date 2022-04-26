using HealthCareCenter.Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace HealthCareCenter
{
    public partial class ManagerWindow : Window
    {
        public Manager signedUser;

        private void fillComboBox()
        {
            RoomTypeComboBox.Items.Add(new ComboBoxItem() { Content = Enums.RoomType.Checkup });
            RoomTypeComboBox.Items.Add(new ComboBoxItem() { Content = Enums.RoomType.Operation });
            RoomTypeComboBox.Items.Add(new ComboBoxItem() { Content = Enums.RoomType.Rest });
            RoomTypeComboBox.Items.Add(new ComboBoxItem() { Content = Enums.RoomType.Other });
            RoomTypeComboBox.SelectedItem = RoomTypeComboBox.Items[0];
        }

        private void fillDataGridHospitalRooms()
        {
            DataGridHospitalRooms.Items.Clear();
            List<HospitalRoom> rooms = HospitalRoomRepository.GetRooms();
            foreach (HospitalRoom room in rooms)
                DataGridHospitalRooms.Items.Add(room);
        }

        private void DeleteHospitalRoom(HospitalRoom delteteRoom)
        {
            HospitalRoomRepository.DeleteRoom(delteteRoom);
            fillDataGridHospitalRooms();
        }

        public ManagerWindow(Model.User user)
        {
            signedUser = (Manager)user;
            InitializeComponent();
            fillComboBox();
            fillDataGridHospitalRooms();
        }

        private void AddHospitalRoomButton_Click(object sender, RoutedEventArgs e)
        {
            Enums.RoomType type = (Enums.RoomType)Enum.Parse(typeof(Enums.RoomType), RoomTypeComboBox.Text);
            string roomName = HospitalRoomNameTextBox.Text;
            if (roomName == "")
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
            int roomId;
            if (!Int32.TryParse(HospitalRoomIdTextBox.Text, out roomId))
            {
                MessageBox.Show("You must enter hospital room Id!");
                return;
            }
            HospitalRoom room = HospitalRoomRepository.GetRoomById(roomId);
            if (room == null)
            {
                MessageBox.Show($"Hospital room with {roomId} Id it's not found!");
                return;
            }

            if (room.EquipmentIDsAmounts.Count != 0)
            {
                MessageBox.Show($"Hospital room with {roomId} Id contain equipment.");
                return;
            }
            if (room.AppointmentIDs.Count != 0)
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
            int roomId;

            if (!Int32.TryParse(HospitalRoomIdTextBox.Text, out roomId))
            {
                MessageBox.Show("You must enter hospital room Id");
                return;
            }

            if (newRoomName == "")
            {
                MessageBox.Show("You must enter room name");
                return;
            }

            HospitalRoom room = HospitalRoomRepository.GetRoomById(roomId);
            if (room == null)
            {
                MessageBox.Show($"Hospital room with {roomId} id it's not found");
                return;
            }

            room.Name = newRoomName;
            room.Type = newType;
            HospitalRoomRepository.UpdateRoom(room);
            fillDataGridHospitalRooms();
            HospitalRoomIdTextBox.Text = "";
            HospitalRoomNameTextBox.Text = "";
        }

        private void ShowWindow(Window window)
        {
            window.Show();
            Close();
        }

        private void CrudHospitalRoomMenuItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new ManagerWindow(signedUser));
        }

        private void EquipmentReviewMenuItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new HospitalEquipmentReviewWindow(signedUser));
        }
    }
}