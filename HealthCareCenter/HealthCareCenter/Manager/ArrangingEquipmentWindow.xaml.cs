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
    /// Interaction logic for ArrangingEquipmentWindow.xaml
    /// </summary>
    public partial class ArrangingEquipmentWindow : Window
    {
        private string[] _headerDataGridEquipment = new string[] { "Equipment Id", "Current Room Id", "Equipment Type", "Equipment Name", "Move Time", "New Room Id" };

        private Manager _signedManager;

        private void ShowWindow(Window window)
        {
            window.Show();
            Close();
        }

        private bool IsNewRoomIdInputValide(string newRoomId)
        {
            return Int32.TryParse(newRoomId, out _);
        }

        private bool IsEquipmentToMoveIdInputValide(string equipmentToMoveId)
        {
            return Int32.TryParse(equipmentToMoveId, out _);
        }

        private bool IsNewRoomStorage(int newRoomId)
        {
            if (newRoomId == 0)
            {
                return true;
            }

            return false;
        }

        private bool IsNewRoomFound(Room room)
        {
            return room != null;
        }

        private bool IsEquipmentToMoveFound(Equipment equipment)
        {
            if (equipment == null)
            {
                return false;
            }

            return true;
        }

        private bool IsDateInputValide(string date, string time)
        {
            return DateTime.TryParse(date + " " + time, out _);
        }

        private bool IsIdOfNewRoomAndCurrentRoomSame(int newRoomId, int currentRoomId)
        {
            return currentRoomId == newRoomId;
        }

        private bool IsEquipmentRearrangementTimeBeforeCurrentTime(DateTime rearrangementDate)
        {
            DateTime now = DateTime.Now;
            int value = DateTime.Compare(rearrangementDate, now);
            return value < 0;
        }

        /// <summary>
        /// Adding DataGridEquipment header
        /// </summary>
        /// <param name="header"></param>
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

        /// <summary>
        /// Adding row in DataGrid.
        /// The row of the DataGrid is obtained by making a string array from object. For example if object is type of equipment and object has arrangement, then the date of arrangement and new room for equipment are added to array like strings, if equipment has no arragement then two empty strings are added to the string array which one represents the row.
        /// </summary>
        /// <param name="equipmentAttributesToDisplay">Content we want to display in DataGridEquipment (One row)</param>
        private void AddDataGridRow(DataGrid dataGrid, string[] header, List<string> equipmentAttributesToDisplay)
        {
            dynamic row = new ExpandoObject();

            for (int i = 0; i < header.Length; i++)
            {
                ((IDictionary<String, Object>)row)[header[i].Replace(' ', '_')] = equipmentAttributesToDisplay[i];
            }

            dataGrid.Items.Add(row);
        }

        /// <summary>
        /// When equipment object don't have rearrangement we add 2 empty strings for "Move Time" and for "New Room Id"
        /// </summary>
        /// <param name="equipmentAttributesToDisplay">Content we want to display in DataGridEquipment</param>
        private void AddEmptyFieldsForEquipmentDisplay(ref List<string> equipmentAttributesToDisplay)
        {
            equipmentAttributesToDisplay.Add("");
            equipmentAttributesToDisplay.Add("");
        }

        /// <summary>
        /// Filling DataGridEquipment with content
        /// </summary>
        private void FillDataGridEquipment()
        {
            List<Equipment> equipments = EquipmentService.GetEquipments();
            foreach (Equipment equipment in equipments)
            {
                if (!equipment.IsScheduledRearrangement())
                {
                    List<string> equipmentAttributesToDisplay = equipment.ToList();
                    AddEmptyFieldsForEquipmentDisplay(ref equipmentAttributesToDisplay);
                    AddDataGridRow(DataGridEquipments, _headerDataGridEquipment, equipmentAttributesToDisplay);
                }
                else
                {
                    List<string> equipmentAttributesToDisplay = equipment.ToList();
                    EquipmentRearrangement rearrangement = EquipmentRearrangementService.GetRearrangement(equipment.RearrangementID);
                    equipmentAttributesToDisplay.Add(rearrangement.MoveTime.ToString(Constants.DateFormat));
                    equipmentAttributesToDisplay.Add(rearrangement.NewRoomID.ToString());
                    AddDataGridRow(DataGridEquipments, _headerDataGridEquipment, equipmentAttributesToDisplay);
                }
            }
        }

        public ArrangingEquipmentWindow(Manager manager)
        {
            _signedManager = manager;
            InitializeComponent();
            TimeComboBox.SelectedItem = TimeComboBox.Items[0];
            AddDataGridHeader(DataGridEquipments, _headerDataGridEquipment);
            FillDataGridEquipment();
        }

        private void TransferButton_Click(object sender, RoutedEventArgs e)
        {
            string newRoomId = NewRoomIdTextBox.Text;
            string movingEquipmentId = EquipmentIdTextBox.Text;

            Room newRoom;
            Equipment equipmentToMove;

            if (!IsNewRoomIdInputValide(newRoomId))
            {
                MessageBox.Show("Error, bad input for new room ID field!");
                return;
            }
            int parsedNewRoomId = Convert.ToInt32(newRoomId);

            if (!IsEquipmentToMoveIdInputValide(movingEquipmentId))
            {
                MessageBox.Show("Error, bad input for equipment ID field!");
                return;
            }
            int parsedMovingEquipmentId = Convert.ToInt32(movingEquipmentId);

            if (IsNewRoomStorage(parsedNewRoomId))
            {
                newRoom = StorageRepository.GetStorage();
            }
            else
            {
                newRoom = (HospitalRoom)RoomService.GetRoom(parsedNewRoomId);
                if (!IsNewRoomFound(newRoom))
                {
                    MessageBox.Show($"Error, room with ID {parsedNewRoomId} not found!");
                    return;
                }
            }

            equipmentToMove = EquipmentService.GetEquipment(parsedMovingEquipmentId);
            if (!IsEquipmentToMoveFound(equipmentToMove))
            {
                MessageBox.Show($"Error, equipment with ID {parsedMovingEquipmentId} not found!");
                return;
            }

            if (equipmentToMove.IsScheduledRearrangement())
            {
                MessageBox.Show("Error, equipment already has scheduled rearrangement!");
                return;
            }

            string rearrangementDate = DatePicker.Text;
            string rearrangementTime = TimeComboBox.Text;
            DateTime rearrangementDateTime = DateTime.Now;
            if (!IsDateInputValide(rearrangementDate, rearrangementTime))
            {
                MessageBox.Show("Error, bad date or time input");
                return;
            }

            rearrangementDateTime = Convert.ToDateTime(rearrangementDate + " " + rearrangementTime);

            if (IsEquipmentRearrangementTimeBeforeCurrentTime(rearrangementDateTime))
            {
                MessageBox.Show("Error, bad date or time input");
                return;
            }

            if (IsIdOfNewRoomAndCurrentRoomSame(parsedNewRoomId, equipmentToMove.CurrentRoomID))
            {
                MessageBox.Show("Current room and new room must be differente!");
                return;
            }

            EquipmentRearrangement rearrangement = new EquipmentRearrangement(equipmentToMove.ID, rearrangementDateTime, equipmentToMove.CurrentRoomID, parsedNewRoomId);
            equipmentToMove.SetRearrangement(rearrangement);
            DataGridEquipments.Items.Clear();
            FillDataGridEquipment();
        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            string movingEquipmentId = EquipmentIdTextBox.Text;

            Equipment equipmentToMove;

            if (!IsEquipmentToMoveIdInputValide(movingEquipmentId))
            {
                MessageBox.Show("Error, bad input for equipment ID field!");
                return;
            }

            int parsedMovingEquipmentId = Convert.ToInt32(movingEquipmentId);
            equipmentToMove = EquipmentService.GetEquipment(parsedMovingEquipmentId);
            if (!IsEquipmentToMoveFound(equipmentToMove))
            {
                MessageBox.Show($"Error, equipment with ID {parsedMovingEquipmentId} not found!");
                return;
            }

            equipmentToMove.RemoveRearrangement();
            DataGridEquipments.Items.Clear();
            FillDataGridEquipment();
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