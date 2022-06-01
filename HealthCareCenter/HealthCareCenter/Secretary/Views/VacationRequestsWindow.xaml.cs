using HealthCareCenter.Enums;
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
using HealthCareCenter.Service;
using HealthCareCenter.Secretary.Controllers;
using System.Collections.ObjectModel;

namespace HealthCareCenter.Secretary
{
    /// <summary>
    /// Interaction logic for VacationRequestsWindow.xaml
    /// </summary>
    public partial class VacationRequestsWindow : Window
    {
        private readonly VacationRequestsController _controller;

        public VacationRequestsWindow()
        {
            _controller = new VacationRequestsController();

            InitializeComponent();

            vacationRequestsDataGrid.ItemsSource = _controller.Get();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (vacationRequestsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("You must select a vacation request first.");
                return;
            }

            int id = ((VacationRequestDisplay)vacationRequestsDataGrid.SelectedItem).ID;
            vacationRequestsDataGrid.ItemsSource = _controller.Accept(id);
            MessageBox.Show("Successfully accepted the vacation request.");
        }

        private void DenyButton_Click(object sender, RoutedEventArgs e)
        {
            if (vacationRequestsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("You must select a vacation request first.");
                return;
            }

            int id = ((VacationRequestDisplay)vacationRequestsDataGrid.SelectedItem).ID;
            try
            {
                vacationRequestsDataGrid.ItemsSource = _controller.Deny(id, denialReasonTextBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            denialReasonTextBox.Clear();
            MessageBox.Show("Successfully denied the vacation request.");
        }
    }
}
