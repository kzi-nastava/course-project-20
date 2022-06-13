using HealthCareCenter.Core.Equipment.Controllers;
using HealthCareCenter.Core.Equipment.Models;
using HealthCareCenter.Core.Rooms.Models;
using HealthCareCenter.Core.Rooms.Services;
using HealthCareCenter.Core.Users.Models;
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

namespace HealthCareCenter
{
    /// <summary>
    /// Interaction logic for EquipmentIrrevocableRearrangementWindow.xaml
    /// </summary>
    public partial class EquipmentIrrevocableRearrangementWindow : Window
    {
        private Manager _signedManager;
        private EquipmentIrrevocableRearrangementController _contoller;

        public EquipmentIrrevocableRearrangementWindow(Manager manager, DateTime finishDate, HospitalRoom splitRoom, HospitalRoom room1, HospitalRoom room2)
        {
            _signedManager = manager;
            List<Equipment> splitRoomEquipments = RoomService.GetAllEquipment(splitRoom);

            _contoller = new EquipmentIrrevocableRearrangementController(splitRoomEquipments, splitRoom, room1, room2, finishDate);

            InitializeComponent();
            FillNewRoomComboBox();
            FillDataGridEquipment();
        }

        private void FillNewRoomComboBox()
        {
            NewRoomComboBox.Items.Add(new ComboBoxItem() { Content = _contoller.Room1.Name });
            NewRoomComboBox.Items.Add(new ComboBoxItem() { Content = _contoller.Room2.Name });
            NewRoomComboBox.SelectedItem = NewRoomComboBox.Items[0];
        }

        private void FillDataGridEquipment()
        {
            DataGridRoomEquipment.Items.Clear();
            foreach (Equipment equipment in _contoller.SplitRoomEquipments)
            {
                DataGridRoomEquipment.Items.Add(equipment);
            }
        }

        private void ShowWindow(Window window)
        {
            window.Show();
            Close();
        }

        private void TransferButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_contoller.IsSplitRoomContainsEquipment())
                {
                    string equipmentId = EquipmentIdTextBox.Text;
                    string newRoom = NewRoomComboBox.Text;
                    _contoller.Transfer(equipmentId, newRoom);
                }

                if (_contoller.IsSplitRoomContainsEquipment())
                {
                    MessageBox.Show("Rearrangement is done!");
                    ShowWindow(new ComplexHospitalRoomRenovationSplitWindow(_signedManager));
                }

                FillDataGridEquipment();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}