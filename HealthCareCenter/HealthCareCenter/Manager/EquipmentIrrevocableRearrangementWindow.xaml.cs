using HealthCareCenter.Model;
using HealthCareCenter.Service;
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
        private List<Equipment> _splitRoomEquipments;
        private HospitalRoom _splitRoom;
        private HospitalRoom _room1;
        private HospitalRoom _room2;
        private DateTime _finishDate;

        private bool IsEqupmentIdInputValide(string equipmentId)
        {
            return Int32.TryParse(equipmentId, out int _);
        }

        private bool IsEquipmentFound(Equipment equipment)
        {
            foreach (Equipment splitRoomEquipment in _splitRoomEquipments)
            {
                if (splitRoomEquipment.ID == equipment.ID)
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsSplitRoomContainsEquipment()
        {
            return _splitRoomEquipments.Count != 0;
        }

        public EquipmentIrrevocableRearrangementWindow(Manager manager, DateTime finshDate, HospitalRoom splitRoom, HospitalRoom room1, HospitalRoom room2)
        {
            _signedManager = manager;
            _splitRoom = splitRoom;
            _finishDate = finshDate;
            _room1 = room1;
            _room2 = room2;
            _splitRoomEquipments = RoomService.GetAllEquipments(_splitRoom);

            InitializeComponent();
            FillNewRoomComboBox();
            FillDataGridEquipment();
        }

        private void FillNewRoomComboBox()
        {
            NewRoomComboBox.Items.Add(new ComboBoxItem() { Content = _room1.Name });
            NewRoomComboBox.Items.Add(new ComboBoxItem() { Content = _room2.Name });
            NewRoomComboBox.SelectedItem = NewRoomComboBox.Items[0];
        }

        private void FillDataGridEquipment()
        {
            DataGridRoomEquipment.Items.Clear();
            foreach (Equipment equipment in _splitRoomEquipments)
            {
                DataGridRoomEquipment.Items.Add(equipment);
            }
        }

        private void ShowWindow(Window window)
        {
            window.Show();
            Close();
        }

        private bool IsEquipmentValide(string equipmentId)
        {
            if (!IsEqupmentIdInputValide(equipmentId))
            {
                MessageBox.Show("Error, bad input for equipment Id!");
                return false;
            }
            int parsedEquipmentId = Convert.ToInt32(equipmentId);
            Equipment equipment = EquipmentService.Get(parsedEquipmentId);
            if (!IsEquipmentFound(equipment))
            {
                MessageBox.Show("Error, equipment not found!");
                return false;
            }
            return true;
        }

        private void SetIrrevocableRearrangement(Equipment equipment, int roomId)
        {
            _splitRoomEquipments.Remove(equipment);
            EquipmentRearrangement rearrangement = new EquipmentRearrangement(equipment, _finishDate, roomId);
            EquipmentRearrangementService.Set(rearrangement, equipment);
        }

        private void DoIrrevocableRearrangement(string newRoom, Equipment equipment)
        {
            if (newRoom == _room1.Name)
            {
                SetIrrevocableRearrangement(equipment, _room1.ID);
            }
            else if (newRoom == _room2.Name)
            {
                SetIrrevocableRearrangement(equipment, _room2.ID);
            }
        }

        private void TransferButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsSplitRoomContainsEquipment())
            {
                string equipmentId = EquipmentIdTextBox.Text;
                string newRoom = NewRoomComboBox.Text;

                if (!IsEquipmentValide(equipmentId)) { return; }
                int parsedEquipmentId = Convert.ToInt32(equipmentId);
                Equipment equipment = EquipmentService.Get(parsedEquipmentId);
                DoIrrevocableRearrangement(newRoom, equipment);
            }

            if (!IsSplitRoomContainsEquipment())
            {
                MessageBox.Show("Rearrangement is done!");
                ShowWindow(new ComplexHospitalRoomRenovationSplitWindow(_signedManager));
            }

            FillDataGridEquipment();
        }
    }
}