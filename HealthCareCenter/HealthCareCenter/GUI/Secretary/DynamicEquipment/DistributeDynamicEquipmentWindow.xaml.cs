using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using HealthCareCenter.Core;
using HealthCareCenter.Core.Equipment.Controllers;
using HealthCareCenter.Core.Equipment.Services;
using HealthCareCenter.Core.Rooms.Models;
using HealthCareCenter.Core.Rooms.Repositories;

namespace HealthCareCenter.Secretary
{
    /// <summary>
    /// Interaction logic for DistributeDynamicEquipmentWindow.xaml
    /// </summary>
    public partial class DistributeDynamicEquipmentWindow : Window
    {
        private readonly Room _storage;
        private readonly DistributeDynamicEquipmentController _controller;

        public DistributeDynamicEquipmentWindow(
            IDynamicEquipmentService dynamicEquipmentService,
            BaseStorageRepository storageRepository)
        {
            _storage = storageRepository.Load();
            _controller = new DistributeDynamicEquipmentController(dynamicEquipmentService);

            InitializeComponent();

            Refresh();
        }

        void Refresh()
        {
            roomsToTransferFromComboBox.Items.Clear();
            roomsWithShortageComboBox.Items.Clear();
            RefreshRoomsWithShortage();
            RefreshTransferFromRooms();
        }

        private void RefreshRoomsWithShortage()
        {
            AddIfHasShortage(_storage);
            foreach (HospitalRoom room in HospitalRoomRepository.Rooms)
            {
                AddIfHasShortage(room);
            }
        }

        private void AddIfHasShortage(Room room)
        {
            bool foundShortage = false;
            foreach (string equipment in Constants.DynamicEquipment)
            {
                if (!room.EquipmentAmounts.ContainsKey(equipment) || room.EquipmentAmounts[equipment] < 5)
                {
                    foundShortage = true;
                    break;
                }
            }
            if (foundShortage)
            {
                AddToShortageRooms(room);
            }
        }

        private void AddToShortageRooms(Room room)
        {
            if (room is HospitalRoom)
            {
                roomsWithShortageComboBox.Items.Add(new
                {
                    ((HospitalRoom)room).Name,
                    ID = room.ID.ToString()
                });
            }
            else
            {
                roomsWithShortageComboBox.Items.Add(new
                {
                    Name = "Storage",
                    ID = room.ID.ToString()
                });
            }
        }

        private void RefreshTransferFromRooms()
        {
            roomsToTransferFromComboBox.Items.Add(new
            {
                Name = "Storage",
                ID = _storage.ID.ToString()
            });
            foreach (HospitalRoom room in HospitalRoomRepository.Rooms)
            {
                roomsToTransferFromComboBox.Items.Add(new
                {
                    room.Name,
                    ID = room.ID.ToString()
                });
            }
        }

        private void RoomsWithShortageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            equipmentFromRoomWithShortageListBox.Items.Clear();
            if (roomsWithShortageComboBox.SelectedItem == null)
            {
                return;
            }

            RefreshEquipment(equipmentFromRoomWithShortageListBox, roomsWithShortageComboBox);
        }

        private void RoomsToTransferFromComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            equipmentFromOtherRoomListBox.Items.Clear();
            if (roomsToTransferFromComboBox.SelectedItem == null)
            {
                return;
            }

            RefreshEquipment(equipmentFromOtherRoomListBox, roomsToTransferFromComboBox);
        }

        private void RefreshEquipment(ListBox listBox, ComboBox comboBox)
        {
            Room room = GetSelectedRoom(comboBox);
            foreach (string equipment in Constants.DynamicEquipment)
            {
                if (!room.EquipmentAmounts.ContainsKey(equipment))
                {
                    listBox.Items.Add(equipment + ": 0");
                }
                else
                {
                    listBox.Items.Add(equipment + ": " + room.EquipmentAmounts[equipment]);
                }
            }
        }

        private Room GetSelectedRoom(ComboBox comboBox)
        {
            int roomID = int.Parse(comboBox.SelectedValue.ToString());
            Room room;

            try
            {
                room = _controller.GetRoom(roomID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

            if (roomID == _storage.ID)
            {
                room = _storage;
            }
            return room;
        }

        private void TransferButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidData())
                return;

            try
            {
                _controller.Transfer(quantityTextBox.Text, equipmentFromOtherRoomListBox.SelectedItem.ToString().Split(":"),
                    GetSelectedRoom(roomsToTransferFromComboBox), GetSelectedRoom(roomsWithShortageComboBox), _storage);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            Reset();
            MessageBox.Show("Successfully transfered the equipment.");
        }

        private bool ValidData()
        {
            if (roomsToTransferFromComboBox.SelectedItem == null)
            {
                MessageBox.Show("You must select a room to transfer equipment from.");
                return false;
            }
            if (roomsWithShortageComboBox.SelectedItem == null)
            {
                MessageBox.Show("You must select a room with an equipment shortage.");
                return false;
            }
            if (equipmentFromOtherRoomListBox.SelectedItem == null)
            {
                MessageBox.Show("You must select equipment to transfer.");
                return false;
            }
            return true;
        }

        private void Reset()
        {
            quantityTextBox.Clear();
            Refresh();
        }
    }

    public class StringToForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && int.Parse(value.ToString().Split(":")[1]) == 0)
                return "Red";
            else
                return "Black";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
