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
        public Manager signedUser;

        public HospitalEquipmentReviewWindow(User user)
        {
            signedUser = (Manager)user;
            InitializeComponent();
            fillDataGridEquipment();
            fillAllComboBoxes();
        }

        /// <summary>
        /// Fill RoomTypeComboBox with room type constants from Enum RoomType.
        /// </summary>
        private void fillRoomTypeComboBox(ComboBox RoomTypeComboBox)
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
        private void fillEquipmentTypeComboBox(ComboBox EquipmentTypeComboBox)
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
        private void fillEquipmentAmountComboBox(ComboBox EquipmentAmountComboBox)
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
        private void fillAllComboBoxes()
        {
            fillRoomTypeComboBox(RoomTypeComboBox);
            fillEquipmentTypeComboBox(EquipmentTypeComboBox);
            fillEquipmentAmountComboBox(EquipmentAmountComboBox);
        }

        /// <summary>
        /// Adding DataGridEquipment header
        /// </summary>
        /// <param name="header"></param>
        private void addDataGridHeader(DataGrid dataGrid, string[] header)
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
        private void addDataGridRow(DataGrid dataGrid, string[] header, List<string> equipmentAttributesToDisplay)
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
        private void addEmptyRowsForEquipmentDisplay(ref List<string> equipmentAttributesToDisplay)
        {
            equipmentAttributesToDisplay.Add("");
            equipmentAttributesToDisplay.Add("");
        }

        /// <summary>
        /// Filling DataGridEquipment with content
        /// </summary>
        private void fillDataGridEquipment()
        {
            addDataGridHeader(DataGridEquipments, _headerDataGridEquipment);

            List<Equipment> equipments = EquipmentRepository.GetEquipments();
            foreach (Equipment equipment in equipments)
            {
                if (equipment.RearrangementID == -1)
                {
                    List<string> equipmentAttributesToDisplay = equipment.ToList();
                    addEmptyRowsForEquipmentDisplay(ref equipmentAttributesToDisplay);
                    addDataGridRow(DataGridEquipments, _headerDataGridEquipment, equipmentAttributesToDisplay);
                }
                else
                {
                    List<string> equipmentAttributesToDisplay = equipment.ToList();
                    EquipmentRearrangement rearrangement = EquipmentRearrangementRepository.GetRearrangementById(equipment.RearrangementID);
                    equipmentAttributesToDisplay.Add(rearrangement.MoveTime.ToString(Constants.DateFormat));
                    equipmentAttributesToDisplay.Add(rearrangement.NewRoomID.ToString());
                    addDataGridRow(DataGridEquipments, _headerDataGridEquipment, equipmentAttributesToDisplay);
                }
            }
        }

        /// <summary>
        /// Filter content by text from search TextBox.
        /// </summary>
        /// <param name="equipmentAttributesToDisplay">Content we are filtering (One row)</param>
        /// <returns>true if contenent pass criterion</returns>
        private bool filterEquipmentsBySearchTextBox(List<string> equipmentAttributesToDisplay)
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
        private bool filterEquipmentsByCurrentRoomType(List<string> equipmentAttributesToDisplay)
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
        private bool filterEquipmentsByEquipmentType(List<string> equipmentAttributesToDisplay)
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
        private bool filterEquipmentsByAmount(List<string> equipmentAttributesToDisplay)
        {
            string amount = EquipmentAmountComboBox.Text;
            string equipmentName = equipmentAttributesToDisplay[3];

            if (amount == "")
                return true;

            Room storage = StorageRepository.GetStorage();

            if (amount == "Out of stock")
                if ((!storage.EquipmentIDsAmounts.ContainsKey(equipmentName)) || (storage.EquipmentIDsAmounts[equipmentName] == 0))
                    return true;

            if (amount == "0-10")
            {
                if (!storage.EquipmentIDsAmounts.ContainsKey(equipmentName))
                    return false;

                if (storage.EquipmentIDsAmounts[equipmentName] > 0 && storage.EquipmentIDsAmounts[equipmentName] < 10)
                    return true;
            }

            if (amount == "10+")
            {
                if (!storage.EquipmentIDsAmounts.ContainsKey(equipmentName))
                    return false;
                if (storage.EquipmentIDsAmounts[equipmentName] > 10)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Here are called all filter metheods and if all filter methods return true, content is added.
        /// </summary>
        /// <param name="equipmentAttributesToDisplay">Content we want to add</param>
        private void filterEquipment(List<string> equipmentAttributesToDisplay)
        {
            if (filterEquipmentsBySearchTextBox(equipmentAttributesToDisplay))
                if (filterEquipmentsByCurrentRoomType(equipmentAttributesToDisplay))
                    if (filterEquipmentsByEquipmentType(equipmentAttributesToDisplay))
                        if (filterEquipmentsByAmount(equipmentAttributesToDisplay))
                            addDataGridRow(DataGridEquipments, _headerDataGridEquipment, equipmentAttributesToDisplay);
        }

        private void ShowSearchResultButton_Click(object sender, RoutedEventArgs e)
        {
            DataGridEquipments.Items.Clear();
            List<Equipment> equipments = EquipmentRepository.GetEquipments();

            foreach (Equipment equipment in equipments)
            {
                if (equipment.RearrangementID == -1)
                {
                    List<string> equipmentAttributesToDisplay = equipment.ToList();
                    addEmptyRowsForEquipmentDisplay(ref equipmentAttributesToDisplay);
                    filterEquipment(equipmentAttributesToDisplay);
                }
                else
                {
                    List<string> equipmentAttributesToDisplay = equipment.ToList();
                    EquipmentRearrangement rearrangement = EquipmentRearrangementRepository.GetRearrangementById(equipment.RearrangementID);
                    equipmentAttributesToDisplay.Add(rearrangement.MoveTime.ToString(Constants.DateFormat));
                    equipmentAttributesToDisplay.Add(rearrangement.NewRoomID.ToString());
                    filterEquipment(equipmentAttributesToDisplay);
                }
            }
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