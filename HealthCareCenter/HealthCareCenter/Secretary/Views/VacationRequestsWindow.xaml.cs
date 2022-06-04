using System;
using System.Windows;
using HealthCareCenter.Secretary.Controllers;

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

            try
            {
                vacationRequestsDataGrid.ItemsSource = _controller.Get();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (vacationRequestsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("You must select a vacation request first.");
                return;
            }

            int id = ((VacationRequestDisplay)vacationRequestsDataGrid.SelectedItem).ID;

            try
            {
                vacationRequestsDataGrid.ItemsSource = _controller.Accept(id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
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
