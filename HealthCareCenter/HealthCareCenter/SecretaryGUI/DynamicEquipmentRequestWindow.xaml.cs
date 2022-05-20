using HealthCareCenter.Model;
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

namespace HealthCareCenter.SecretaryGUI
{
    /// <summary>
    /// Interaction logic for DynamicEquipmentRequestWindow.xaml
    /// </summary>
    public partial class DynamicEquipmentRequestWindow : Window
    {
        private Secretary _secretary;
        private List<string> _request;

        public DynamicEquipmentRequestWindow()
        {
            InitializeComponent();
        }

        public DynamicEquipmentRequestWindow(Secretary secretary)
        {
            _secretary = secretary;
            _request = new List<string>();

            InitializeComponent();

            Refresh();

            DynamicEquipmentRequestService.CalculateMaxID();
        }

        private void Refresh()
        {
            RefreshMissingEquipment();
            allEquipmentComboBox.ItemsSource = Constants.DynamicEquipment;
            requestedEquipmentListBox.ItemsSource = _request;
        }

        private void RefreshMissingEquipment()
        {
            Room storage = StorageRepository.Load();
            List<string> missingEquipment = new List<string>(Constants.DynamicEquipment);
            foreach (string equipment in storage.EquipmentAmounts.Keys)
            {
                if (storage.EquipmentAmounts[equipment] > 0)
                {
                    missingEquipment.Remove(equipment);
                }
            }
            missingEquipmentListBox.ItemsSource = missingEquipment;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(quantityTextBox.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Quantity must be a positive number.");
                return;
            }
            if (allEquipmentComboBox.SelectedItem == null)
            {
                MessageBox.Show("You must select the dynamic equipment that you wish to add to the request.");
                return;
            }
            if (IsAlreadyAdded())
            {
                MessageBox.Show("You cannot add equipment that you have already added.");
                return;
            }

            _request.Add(allEquipmentComboBox.Text + ": " + quantity);
            requestedEquipmentListBox.Items.Refresh();
        }

        private bool IsAlreadyAdded()
        {
            string selectedEquipment = allEquipmentComboBox.Text.Split(":")[0];
            foreach (string equipmentWithQuantity in _request)
            {
                string equipment = equipmentWithQuantity.Split(":")[0];
                if (selectedEquipment == equipment)
                {
                    return true;
                }
            }
            return false;
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (requestedEquipmentListBox.SelectedItem == null)
            {
                MessageBox.Show("You must select equipment from the request that you wish to remove.");
                return;
            }

            _request.Remove(requestedEquipmentListBox.SelectedItem.ToString());
            requestedEquipmentListBox.Items.Refresh();
        }

        private void SendRequestButton_Click(object sender, RoutedEventArgs e)
        {
            if (_request.Count == 0)
            {
                MessageBox.Show("You must first add equipment to the request.");
                return;
            }

            Dictionary<string, int> amountOfEquipment = getAmountOfEquipment();
            SendRequest(amountOfEquipment);
            Reset();
            MessageBox.Show("Successfully sent a request for acquisition of dynamic equipment.");
        }

        private void Reset()
        {
            allEquipmentComboBox.Text = "";
            quantityTextBox.Text = "";
            _request = new List<string>();
            requestedEquipmentListBox.ItemsSource = _request;
        }

        private void SendRequest(Dictionary<string, int> amountOfEquipment)
        {
            DynamicEquipmentRequest request = new DynamicEquipmentRequest(++DynamicEquipmentRequestService.maxID, false, _secretary.ID, DateTime.Now, amountOfEquipment);
            DynamicEquipmentRequestRepository.Requests.Add(request);
            DynamicEquipmentRequestRepository.Save();
        }

        private Dictionary<string, int> getAmountOfEquipment()
        {
            Dictionary<string, int> amountOfEquipment = new Dictionary<string, int>();

            foreach (string equipmentWithQuantity in _request)
            {
                var equipmentAndQuantity = equipmentWithQuantity.Split(":");
                string equipment = equipmentAndQuantity[0];
                int quantity = int.Parse(equipmentAndQuantity[1]);
                amountOfEquipment.Add(equipment, quantity);
            }

            return amountOfEquipment;
        }
    }
}
