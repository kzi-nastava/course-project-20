using HealthCareCenter.Controller;
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
        private CRUDHospitalRoomController _CRUDcontroler = new CRUDHospitalRoomController();

        public CrudHospitalRoomWindow(Manager manager)
        {
            _signedManager = manager;
            InitializeComponent();
            FillRoomTypeComboBox();
            FillDataGridHospitalRooms();
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
            List<HospitalRoom> roomsForDisplay = _CRUDcontroler.GetRoomsToDisplay();
            foreach (HospitalRoom room in roomsForDisplay)
            {
                DataGridHospitalRooms.Items.Add(room);
            }
        }

        private void UpdateInterfaceAfterDeleteOfHospitalRoom()
        {
            FillDataGridHospitalRooms();
            SetAllTextBoxesToBeBlank();
        }

        private void UpdateInterfaceAfterAddingHospitalRoom(HospitalRoom room)
        {
            DataGridHospitalRooms.Items.Add(room);
            SetAllTextBoxesToBeBlank();
        }

        private void UpdateInterfaceAfterUpdateOfHospitalRoom()
        {
            FillDataGridHospitalRooms();
            SetAllTextBoxesToBeBlank();
        }

        private void AddHospitalRoomButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Enums.RoomType type = (Enums.RoomType)Enum.Parse(typeof(Enums.RoomType), RoomTypeComboBox.Text);
                string roomName = HospitalRoomNameTextBox.Text;

                _CRUDcontroler.Create(type, roomName);
                HospitalRoom room = new HospitalRoom(type, roomName);
                UpdateInterfaceAfterAddingHospitalRoom(room);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteHospitalRoomButton_Click(object sender, RoutedEventArgs e)

        {
            try
            {
                string roomId = HospitalRoomIdTextBox.Text;
                _CRUDcontroler.Delete(roomId);
                UpdateInterfaceAfterDeleteOfHospitalRoom();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateHospitalRoomButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Enums.RoomType newType = (Enums.RoomType)Enum.Parse(typeof(Enums.RoomType), RoomTypeComboBox.Text);
                string newRoomName = HospitalRoomNameTextBox.Text;
                string roomId = HospitalRoomIdTextBox.Text;

                _CRUDcontroler.Update(newRoomName, newType, roomId);
                UpdateInterfaceAfterUpdateOfHospitalRoom();
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
            ShowWindow(new MedicineCreationRequestWindow(_signedManager));
        }

        private void ReffusedMedicineClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new ChangedMedicineCreationRequestWindow(_signedManager));
        }

        private void LogOffItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new LoginWindow());
        }
    }
}