using HealthCareCenter.Model;
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

        private Manager _signedUser;

        private void ShowWindow(Window window)
        {
            window.Show();
            Close();
        }

        private bool IsRearrangementAllowed(Equipment equipmentToMove, Room room)
        {
            if (room.IsStorage())
                return true;
            else
            {
                HospitalRoom hospitalRoom = (HospitalRoom)room;

                if (equipmentToMove.Type == Enums.EquipmentType.ForCheckup)
                    if (hospitalRoom.Type != Enums.RoomType.Checkup)
                        return false;

                if (equipmentToMove.Type == Enums.EquipmentType.ForSurgery)
                    if (hospitalRoom.Type != Enums.RoomType.Operation)
                        return false;

                if (hospitalRoom.Type == Enums.RoomType.Checkup)
                    if (equipmentToMove.Type != Enums.EquipmentType.ForCheckup)

                        return false;

                if (hospitalRoom.Type == Enums.RoomType.Operation)
                    if (equipmentToMove.Type == Enums.EquipmentType.ForSurgery)
                        return false;

                if (hospitalRoom.Type == Enums.RoomType.Rest)
                    if (equipmentToMove.Type != Enums.EquipmentType.Furniture)
                        return false;
            }

            return true;
        }

        private bool IsNewRoomIdInputValide(ref int newRoomId)
        {
            if (!Int32.TryParse(NewRoomIdTextBox.Text, out newRoomId))
                return false;

            return true;
        }

        private bool IsEquipmentToMoveIdInputValide(ref int EquipmentToMoveId)
        {
            if (!Int32.TryParse(EquipmentIdTextBox.Text, out EquipmentToMoveId))
                return false;

            return true;
        }

        private bool IsNewRoomStorage(int newRoomId)
        {
            if (newRoomId == 0)
                return true;

            return false;
        }

        private bool IsNewRoomFound(Room room)
        {
            if (room == null)
                return false;

            return true;
        }

        private bool IsEquipmentToMoveFound(Equipment equipment)
        {
            if (equipment == null)
                return false;

            return true;
        }

        private bool IsDateInputValide(ref DateTime equipmentToMoveDate)
        {
            string date = DatePicker.Text;
            string time = TimeComboBox.Text;
            if (!DateTime.TryParse(date + " " + time, out equipmentToMoveDate))
                return false;

            return true;
        }

        private bool IsIdOfNewRoomAndCurrentRoomSame(int newRoomId, int currentRoomId)
        {
            if (currentRoomId == newRoomId)
                return true;

            return false;
        }

        private bool IsEquipmentRearrangementTimeBeforeCurrentTime(DateTime rearrangementDate)
        {
            DateTime now = DateTime.Now;
            int value = DateTime.Compare(rearrangementDate, now);
            if (value < 0)
                return true;

            return false;
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
                ((IDictionary<String, Object>)row)[header[i].Replace(' ', '_')] = equipmentAttributesToDisplay[i];
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
            List<Equipment> equipments = EquipmentRepository.GetEquipments();
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
                    EquipmentRearrangement rearrangement = EquipmentRearrangementRepository.GetRearrangementById(equipment.RearrangementID);
                    equipmentAttributesToDisplay.Add(rearrangement.MoveTime.ToString(Constants.DateFormat));
                    equipmentAttributesToDisplay.Add(rearrangement.NewRoomID.ToString());
                    AddDataGridRow(DataGridEquipments, _headerDataGridEquipment, equipmentAttributesToDisplay);
                }
            }
        }

        public ArrangingEquipmentWindow(Manager manager)
        {
            _signedUser = manager;
            InitializeComponent();
            TimeComboBox.SelectedItem = TimeComboBox.Items[0];
            AddDataGridHeader(DataGridEquipments, _headerDataGridEquipment);
            FillDataGridEquipment();
        }

        private void TransferButton_Click(object sender, RoutedEventArgs e)
        {
            int newRoomId = 0;
            int movingEquipmentId = 0;

            Room newRoom;
            Equipment equipmentToMove;

            if (!IsNewRoomIdInputValide(ref newRoomId))
            {
                MessageBox.Show("Error, bad input for new room ID field!");
                return;
            }

            if (!IsEquipmentToMoveIdInputValide(ref movingEquipmentId))
            {
                MessageBox.Show("Error, bad input for equipment ID field!");
                return;
            }

            if (IsNewRoomStorage(newRoomId))
                newRoom = StorageRepository.GetStorage();
            else
            {
                newRoom = HospitalRoomRepository.GetRoomById(newRoomId);
                if (!IsNewRoomFound(newRoom))
                {
                    MessageBox.Show($"Error, room with ID {newRoomId} not found!");
                    return;
                }
            }

            equipmentToMove = EquipmentRepository.GetEquipmentById(movingEquipmentId);
            if (!IsEquipmentToMoveFound(equipmentToMove))
            {
                MessageBox.Show($"Error, equipment with ID {movingEquipmentId} not found!");
                return;
            }

            if (equipmentToMove.IsScheduledRearrangement())
            {
                MessageBox.Show("Error, equipment already has scheduled rearrangement!");
                return;
            }

            DateTime rearrangementDate = DateTime.Now;
            if (!IsDateInputValide(ref rearrangementDate))
            {
                MessageBox.Show("Error, bad date or time input");
                return;
            }

            if (IsEquipmentRearrangementTimeBeforeCurrentTime(rearrangementDate))
            {
                MessageBox.Show("Error, bad date or time input");
                return;
            }

            if (IsIdOfNewRoomAndCurrentRoomSame(newRoomId, equipmentToMove.CurrentRoomID))
            {
                MessageBox.Show("Current room and new room must be differente!");
                return;
            }

            //if (!isRearrangementAllowed(equipmentToMove, newRoom))
            //{
            //    MessageBox.Show("Error, rearrangement not allowed!");
            //}

            EquipmentRearrangement rearrangement = new EquipmentRearrangement(equipmentToMove.ID, rearrangementDate, equipmentToMove.CurrentRoomID, newRoomId);
            equipmentToMove.SetRearrangement(rearrangement);
            DataGridEquipments.Items.Clear();
            FillDataGridEquipment();
        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            int movingEquipmentId = 0;

            Equipment equipmentToMove;

            if (!IsEquipmentToMoveIdInputValide(ref movingEquipmentId))
            {
                MessageBox.Show("Error, bad input for equipment ID field!");
                return;
            }

            equipmentToMove = EquipmentRepository.GetEquipmentById(movingEquipmentId);
            if (!IsEquipmentToMoveFound(equipmentToMove))
            {
                MessageBox.Show($"Error, equipment with ID {movingEquipmentId} not found!");
                return;
            }

            equipmentToMove.RemoveRearrangement();
            DataGridEquipments.Items.Clear();
            FillDataGridEquipment();
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