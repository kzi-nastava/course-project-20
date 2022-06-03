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
        private string[] _headerDataGridEquipment = new string[]
        {
            "Equipment Id", "Current Room Id", "Equipment Type",
            "Equipment Name", "Move Time", "New Room Id"
        };

        private Manager _signedManager;

        private bool IsNewRoomIdInputValide(string newRoomId)
        {
            return Int32.TryParse(newRoomId, out _);
        }

        private bool IsEquipmentForRearrangementIdInputValide(string equipmentForRearrangementId)
        {
            return Int32.TryParse(equipmentForRearrangementId, out _);
        }

        private bool IsNewRoomStorage(int newRoomId)
        {
            if (newRoomId == 0)
            {
                return true;
            }

            return false;
        }

        private bool IsNewRoomFound(Room mewRoom)
        {
            return mewRoom != null;
        }

        private bool IsEquipmentRearrangementFound(EquipmentRearrangement rearrangement)
        {
            if (rearrangement == null)
            {
                return false;
            }
            return true;
        }

        private bool IsEquipmentForRearrangementFound(Equipment equipment)
        {
            return equipment != null;
        }

        private bool IsDateTimeInputValide(string date, string time)
        {
            return DateTime.TryParse(date + " " + time, out _);
        }

        private bool WhetherRoomsAreSame(int newRoomId, int currentRoomId)
        {
            return currentRoomId == newRoomId;
        }

        private bool IsDateTimeBeforeCurrentDateTime(DateTime rearrangementDateTime)
        {
            DateTime now = DateTime.Now;
            int value = DateTime.Compare(rearrangementDateTime, now);
            return value < 0;
        }

        private bool IsNewRoomValide(string newRoomId)
        {
            if (!IsNewRoomIdInputValide(newRoomId))
            {
                MessageBox.Show("Error, bad input for new room ID field!");
                return false;
            }
            int parsedNewRoomId = Convert.ToInt32(newRoomId);

            Room newRoom = (HospitalRoom)RoomService.Get(parsedNewRoomId);
            if (!IsNewRoomFound(newRoom))
            {
                MessageBox.Show($"Error, room with ID {parsedNewRoomId} not found!");
                return false;
            }

            return true;
        }

        private bool IsEquipmentFroRearrangementValide(string equipmentId)
        {
            if (!IsEquipmentForRearrangementIdInputValide(equipmentId))
            {
                MessageBox.Show("Error, bad input for equipment ID field!");
                return false;
            }

            int parsedEquipmentForRearrangementId = Convert.ToInt32(equipmentId);
            Equipment equipmentForRearrangement = EquipmentService.Get(parsedEquipmentForRearrangementId);

            if (!IsEquipmentForRearrangementFound(equipmentForRearrangement))
            {
                MessageBox.Show($"Error, equipment with ID {parsedEquipmentForRearrangementId} not found!");
                return false;
            }

            if (equipmentForRearrangement.IsScheduledRearrangement())
            {
                MessageBox.Show("Error, equipment already has scheduled rearrangement!");
                return false;
            }

            return true;
        }

        private bool IsDateTimeValide(string rearrangementDate, string rearrangementTime)
        {
            if (!IsDateTimeInputValide(rearrangementDate, rearrangementTime))
            {
                MessageBox.Show("Error, bad date or time input");
                return false;
            }

            DateTime rearrangementDateTime = Convert.ToDateTime(rearrangementDate + " " + rearrangementTime);

            if (IsDateTimeBeforeCurrentDateTime(rearrangementDateTime))
            {
                MessageBox.Show("Error, date is before currnet date!");
                return false;
            }

            return true;
        }

        private bool IsPossibleRearrangement(EquipmentRearrangement rearrangement)
        {
            if (WhetherRoomsAreSame(rearrangement.NewRoomID, rearrangement.OldRoomID))
            {
                MessageBox.Show("Current room and new room must be differente!");
                return false;
            }

            // Checking are rooms available
            HospitalRoom currentRoom = HospitalRoomService.Get(rearrangement.OldRoomID);
            HospitalRoom newRoom = HospitalRoomService.Get(rearrangement.NewRoomID);
            if (currentRoom == null)
            {
                if (rearrangement.OldRoomID != 0)
                {
                    MessageBox.Show($"Error, current room with id={rearrangement.OldRoomID} is renovating");
                    return false;
                }
            }
            if (newRoom == null)
            {
                if (rearrangement.NewRoomID != 0)
                {
                    MessageBox.Show($"Error, new room with id={rearrangement.NewRoomID} is renovating!");
                    return false;
                }
            }

            return true;
        }

        private bool IsEqipmentForUndoigRearrangementValide(string equipmentId)
        {
            if (!IsEquipmentForRearrangementIdInputValide(equipmentId))
            {
                MessageBox.Show("Error, bad input for equipment ID field!");
                return false;
            }

            int parsedEquipmentForRearrangementId = Convert.ToInt32(equipmentId);
            Equipment equipmentForRearrangement = EquipmentService.Get(parsedEquipmentForRearrangementId);
            if (!IsEquipmentForRearrangementFound(equipmentForRearrangement))
            {
                MessageBox.Show($"Error, equipment with ID {parsedEquipmentForRearrangementId} not found!");
                return false;
            }

            return true;
        }

        private bool IsPossibleToUndoEquipmentRearrangement(EquipmentRearrangement rearrangement, Equipment equipment)
        {
            if (IsEquipmentRearrangementFound(rearrangement))
            {
                MessageBox.Show($"Error, rearrangement for eqiupment with id={equipment.ID} is not found!");
                return false;
            }
            if (rearrangement.IsIrrevocable())
            {
                MessageBox.Show("Error, rerrangement is irrevocable");
                return false;
            }

            return true;
        }

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
            string equipmentForRearrangementId = EquipmentIdTextBox.Text;
            string rearrangementDate = DatePicker.Text;
            string rearrangementTime = TimeComboBox.Text;

            if (!IsNewRoomValide(newRoomId))
            {
                return;
            }
            int parsedNewRoomId = Convert.ToInt32(newRoomId);

            if (!IsEquipmentFroRearrangementValide(equipmentForRearrangementId))
            {
                return;
            }
            int parsedEquipmentForRearrangementId = Convert.ToInt32(equipmentForRearrangementId);
            Equipment equipmentForRearrangement = EquipmentService.Get(parsedEquipmentForRearrangementId);

            if (!IsDateTimeInputValide(rearrangementDate, rearrangementTime))
            {
                return;
            }
            DateTime rearrangementDateTime = Convert.ToDateTime(rearrangementDate + " " + rearrangementTime);

            EquipmentRearrangement rearrangement = new EquipmentRearrangement(
                equipmentForRearrangement.ID, rearrangementDateTime,
                equipmentForRearrangement.CurrentRoomID, parsedNewRoomId);
            if (!IsPossibleRearrangement(rearrangement))
            {
                return;
            }

            equipmentForRearrangement.SetRearrangement(rearrangement);

            DataGridEquipments.Items.Clear();
            FillDataGridEquipment();
        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            string equipmentForRearrangementId = EquipmentIdTextBox.Text;
            if (!IsEqipmentForUndoigRearrangementValide(equipmentForRearrangementId))
            {
                return;
            }
            int parsedEquipmentForRearrangementId = Convert.ToInt32(equipmentForRearrangementId);
            Equipment equipmentForRearrangement = EquipmentService.Get(parsedEquipmentForRearrangementId);

            EquipmentRearrangement rearrangement = EquipmentRearrangementService.GetRearrangement(equipmentForRearrangement.RearrangementID);
            if (!IsPossibleToUndoEquipmentRearrangement(rearrangement, equipmentForRearrangement))
            {
                return;
            }

            equipmentForRearrangement.RemoveRearrangement();

            DataGridEquipments.Items.Clear();
            FillDataGridEquipment();
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
            ShowWindow(new MedicineCreationWindow(_signedManager));
        }

        private void LogOffItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new LoginWindow());
        }
    }
}