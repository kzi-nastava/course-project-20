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
using HealthCareCenter.Model;

namespace HealthCareCenter
{
    /// <summary>
    /// Interaction logic for HospitalEquipmentReviewWindow.xaml
    /// </summary>
    public partial class HospitalEquipmentReviewWindow : Window
    {
        private string[] _headerDataGridEquipment = new string[] { "Equipment Id", "Current Room Id", "Equipment Type", "Equipment Name", "Move Time", "New Room Id" };
        private Manager _signedUser;

        public HospitalEquipmentReviewWindow(User user)
        {
            _signedUser = (Manager)user;
            InitializeComponent();
            AddDataGridHeader(DataGridEquipments, _headerDataGridEquipment);
            FillDataGridEquipment();
            FillAllComboBoxes();
        }

        private void ShowWindow(Window window)
        {
            window.Show();
            Close();
        }

        /// <summary>
        /// Fill RoomTypeComboBox with room type constants from Enum RoomType.
        /// </summary>
        private void FillRoomTypeComboBox(ComboBox RoomTypeComboBox)
        {
            RoomTypeComboBox.Items.Add(new ComboBoxItem() { Content = "" });
            RoomTypeComboBox.Items.Add(new ComboBoxItem() { Content = Enums.RoomType.Storage });
            RoomTypeComboBox.Items.Add(new ComboBoxItem() { Content = Enums.RoomType.Checkup });
            RoomTypeComboBox.Items.Add(new ComboBoxItem() { Content = Enums.RoomType.Operation });
            RoomTypeComboBox.Items.Add(new ComboBoxItem() { Content = Enums.RoomType.Rest });
            RoomTypeComboBox.Items.Add(new ComboBoxItem() { Content = Enums.RoomType.Other });
            RoomTypeComboBox.SelectedItem = RoomTypeComboBox.Items[0];
        }

        /// <summary>
        ///  Fill EquipmentTypeComboBox with equipment type constants from Enum EquipmentType.
        /// </summary>
        private void FillEquipmentTypeComboBox(ComboBox EquipmentTypeComboBox)
        {
            EquipmentTypeComboBox.Items.Add(new ComboBoxItem() { Content = "" });
            EquipmentTypeComboBox.Items.Add(new ComboBoxItem() { Content = Enums.EquipmentType.ForCheckup });
            EquipmentTypeComboBox.Items.Add(new ComboBoxItem() { Content = Enums.EquipmentType.ForHallway });
            EquipmentTypeComboBox.Items.Add(new ComboBoxItem() { Content = Enums.EquipmentType.ForSurgery });
            EquipmentTypeComboBox.Items.Add(new ComboBoxItem() { Content = Enums.EquipmentType.Furniture });
            EquipmentTypeComboBox.SelectedItem = EquipmentTypeComboBox.Items[0];
        }

        /// <summary>
        ///  Fill EquipmentAmountComboBox with amount constants given in project specification #1.2.
        /// </summary>
        private void FillEquipmentAmountComboBox(ComboBox EquipmentAmountComboBox)
        {
            EquipmentAmountComboBox.Items.Add(new ComboBoxItem() { Content = "" });
            EquipmentAmountComboBox.Items.Add(new ComboBoxItem() { Content = "Out of stock" });
            EquipmentAmountComboBox.Items.Add(new ComboBoxItem() { Content = "0-10" });
            EquipmentAmountComboBox.Items.Add(new ComboBoxItem() { Content = "10+" });
            EquipmentAmountComboBox.SelectedItem = EquipmentAmountComboBox.Items[0];
        }

        /// <summary>
        /// This method behaves like a container that calls only the other  methods for filling all comboBoxes.
        /// </summary>
        private void FillAllComboBoxes()
        {
            FillRoomTypeComboBox(RoomTypeComboBox);
            FillEquipmentTypeComboBox(EquipmentTypeComboBox);
            FillEquipmentAmountComboBox(EquipmentAmountComboBox);
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

        /// <summary>
        /// Filter content by text from search TextBox.
        /// </summary>
        /// <param name="equipmentAttributesToDisplay">Content we are filtering (One row)</param>
        /// <returns>true if contenent pass criterion</returns>
        private bool FilterEquipmentsBySearchTextBox(List<string> equipmentAttributesToDisplay)
        {
            string searchContent = SearchEquipmentTextBox.Text;
            if (searchContent == "")
                return true;
            foreach (string attribute in equipmentAttributesToDisplay)
            {
                if (attribute.Contains(searchContent))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Filter content by type of current room (the room in which equipment) is located
        /// </summary>
        /// <param name="equipmentAttributesToDisplay">Content we want to fileter</param>
        /// <returns>True if content pass criterion</returns>
        private bool FilterEquipmentsByCurrentRoomType(List<string> equipmentAttributesToDisplay)
        {
            string roomType = RoomTypeComboBox.Text;
            string currentRoomId = equipmentAttributesToDisplay[1];

            if (roomType == "")
                return true;

            if ((roomType == "Storage") && (currentRoomId == "0"))
                return true;
            else if (currentRoomId != "0")
            {
                HospitalRoom room = HospitalRoomRepository.GetRoomById(Convert.ToInt32(currentRoomId));
                if (roomType == room.Type.ToString())
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Filter content by equipment type
        /// </summary>
        /// <param name="equipmentAttributesToDisplay">Contet we want to fileter</param>
        /// <returns>True if content pass criterion</returns>
        private bool FilterEquipmentsByEquipmentType(List<string> equipmentAttributesToDisplay)
        {
            string equipmentType = EquipmentTypeComboBox.Text;

            if (equipmentType == "")
                return true;

            if (equipmentAttributesToDisplay[2].Contains(equipmentType))
                return true;

            return false;
        }

        /// <summary>
        /// Filter content by his ammount in storage
        /// </summary>
        /// <param name="equipmentAttributesToDisplay">Content we want to filter</param>
        /// <returns>True if content pass criterion</returns>
        private bool FilterEquipmentsByAmount(List<string> equipmentAttributesToDisplay)
        {
            string amount = EquipmentAmountComboBox.Text;
            string equipmentName = equipmentAttributesToDisplay[3];

            if (amount == "")
                return true;

            Room storage = StorageRepository.GetStorage();

            if (amount == "Out of stock")
                if (!storage.ContainsEquipmentName(equipmentName))
                    return true;

            if (amount == "0-10")
            {
                if (!storage.ContainsEquipmentName(equipmentName))
                    return false;

                if (storage.GetEquipmentAmount(equipmentName) > 0 && storage.GetEquipmentAmount(equipmentName) < 10)
                    return true;
            }

            if (amount == "10+")
            {
                if (!storage.ContainsEquipmentName(equipmentName))
                    return false;
                if (storage.GetEquipmentAmount(equipmentName) > 10)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Here are called all filter metheods and if all filter methods return true, content is added.
        /// </summary>
        /// <param name="equipmentAttributesToDisplay">Content we want to add</param>
        private void FilterEquipment(List<string> equipmentAttributesToDisplay)
        {
            if (FilterEquipmentsBySearchTextBox(equipmentAttributesToDisplay))
                if (FilterEquipmentsByCurrentRoomType(equipmentAttributesToDisplay))
                    if (FilterEquipmentsByEquipmentType(equipmentAttributesToDisplay))
                        if (FilterEquipmentsByAmount(equipmentAttributesToDisplay))
                            AddDataGridRow(DataGridEquipments, _headerDataGridEquipment, equipmentAttributesToDisplay);
        }

        private void ShowSearchResultButton_Click(object sender, RoutedEventArgs e)
        {
            DataGridEquipments.Items.Clear();
            List<Equipment> equipments = EquipmentRepository.GetEquipments();

            foreach (Equipment equipment in equipments)
            {
                if (!equipment.IsScheduledRearrangement())
                {
                    List<string> equipmentAttributesToDisplay = equipment.ToList();
                    AddEmptyFieldsForEquipmentDisplay(ref equipmentAttributesToDisplay);
                    FilterEquipment(equipmentAttributesToDisplay);
                }
                else
                {
                    List<string> equipmentAttributesToDisplay = equipment.ToList();
                    EquipmentRearrangement rearrangement = EquipmentRearrangementRepository.GetRearrangementById(equipment.RearrangementID);
                    equipmentAttributesToDisplay.Add(rearrangement.MoveTime.ToString(Constants.DateFormat));
                    equipmentAttributesToDisplay.Add(rearrangement.NewRoomID.ToString());
                    FilterEquipment(equipmentAttributesToDisplay);
                }
            }
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