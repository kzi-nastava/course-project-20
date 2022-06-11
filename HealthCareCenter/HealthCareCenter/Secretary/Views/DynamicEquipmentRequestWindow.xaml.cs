using HealthCareCenter.Model;
using System;
using System.Collections.Generic;
using System.Windows;
using HealthCareCenter.Secretary.Controllers;
using HealthCareCenter.Service;

namespace HealthCareCenter.Secretary
{
    /// <summary>
    /// Interaction logic for DynamicEquipmentRequestWindow.xaml
    /// </summary>
    public partial class DynamicEquipmentRequestWindow : Window
    {
        private readonly Model.Secretary _secretary;
        private List<string> _request;
        private readonly DynamicEquipmentRequestController _controller;

        public DynamicEquipmentRequestWindow()
        {
            InitializeComponent();
        }

        public DynamicEquipmentRequestWindow(Model.Secretary secretary, IDynamicEquipmentService service)
        {
            _secretary = secretary;
            _request = new List<string>();
            _controller = new DynamicEquipmentRequestController(service);

            InitializeComponent();

            Refresh();
        }

        private void Refresh()
        {
            missingEquipmentListBox.ItemsSource = _controller.GetMissingEquipment();
            allEquipmentComboBox.ItemsSource = Constants.DynamicEquipment;
            requestedEquipmentListBox.ItemsSource = _request;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (allEquipmentComboBox.SelectedItem == null)
            {
                MessageBox.Show("You must select the dynamic equipment that you wish to add to the request.");
                return;
            }

            int quantity;
            try
            {
                quantity = _controller.Add(allEquipmentComboBox.Text.Split(":")[0], quantityTextBox.Text, _request);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            _request.Add(allEquipmentComboBox.Text + ": " + quantity);
            requestedEquipmentListBox.Items.Refresh();
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
            try
            {
                _controller.Send(_request, _secretary);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

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
    }
}
