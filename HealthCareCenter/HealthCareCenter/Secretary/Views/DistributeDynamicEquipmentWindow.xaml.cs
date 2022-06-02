using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HealthCareCenter.Model;
using HealthCareCenter.Service;

namespace HealthCareCenter.Secretary
{
    /// <summary>
    /// Interaction logic for DistributeDynamicEquipmentWindow.xaml
    /// </summary>
    public partial class DistributeDynamicEquipmentWindow : Window
    {
        readonly Room _storage;
        public DistributeDynamicEquipmentWindow()
        {
            _storage = StorageRepository.Load();

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
            Room room = HospitalRoomService.Get(roomID);
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

            if (!int.TryParse(quantityTextBox.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Quantity must be a positive number.");
                return;
            }

            string[] equipmentAndAmount = equipmentFromOtherRoomListBox.SelectedItem.ToString().Split(":");
            string selectedEquipment = equipmentAndAmount[0];
            int selectedEquipmentAmount = int.Parse(equipmentAndAmount[1]);

            if (quantity > selectedEquipmentAmount)
            {
                MessageBox.Show("You cannot enter a quantity bigger than the amount of equipment available in the room.");
                return;
            }

            Transfer(quantity, selectedEquipment);
            Reset();
            MessageBox.Show("Successfully transfered the equipment.");
        }

        private void Transfer(int quantity, string selectedEquipment)
        {
            Room roomWithEquipment = GetSelectedRoom(roomsToTransferFromComboBox);
            Room roomWithShortage = GetSelectedRoom(roomsWithShortageComboBox);

            roomWithEquipment.EquipmentAmounts[selectedEquipment] -= quantity;

            if (roomWithShortage.EquipmentAmounts.ContainsKey(selectedEquipment))
                roomWithShortage.EquipmentAmounts[selectedEquipment] += quantity;
            else
                roomWithShortage.EquipmentAmounts.Add(selectedEquipment, quantity);

            StorageRepository.Save(_storage);
            HospitalRoomRepository.Save();
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
            if (roomsToTransferFromComboBox.Text == roomsWithShortageComboBox.Text)
            {
                MessageBox.Show("You must select different rooms.");
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
