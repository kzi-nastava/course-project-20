using HealthCareCenter.Enums;
using HealthCareCenter.Model;
using HealthCareCenter.Secretary.Controllers;
using HealthCareCenter.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HealthCareCenter.Secretary
{
    /// <summary>
    /// Interaction logic for ViewChangeRequestsWindow.xaml
    /// </summary>
    public partial class ViewChangeRequestsWindow : Window
    {
        private readonly Patient _patient;

        private List<DeleteRequest> _deleteRequests;
        private List<EditRequest> _editRequests;

        private readonly ViewChangeRequestsController _controller;

        public ViewChangeRequestsWindow()
        {
            InitializeComponent();
        }

        public ViewChangeRequestsWindow(Patient patient)
        {
            this._patient = patient;

            _controller = new ViewChangeRequestsController();

            InitializeComponent();

            Refresh();
        }

        private void Refresh()
        {
            _deleteRequests = new List<DeleteRequest>();
            _editRequests = new List<EditRequest>();

            _controller.Refresh(_deleteRequests, _editRequests, _patient);

            deleteRequestsDataGrid.ItemsSource = _deleteRequests;
            editRequestsDataGrid.ItemsSource = _editRequests;
        }

        private void AcceptDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (deleteRequestsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("You must select a delete request from the table first.");
                return;
            }

            int requestID = (int)((DeleteRequest)deleteRequestsDataGrid.SelectedItem).ID;
            try
            {
                _controller.AcceptDeleteRequest(requestID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            Refresh();
            MessageBox.Show("Successfully accepted.");
        }

        private void RejectDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (deleteRequestsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("You must select a delete request from the table first.");
                return;
            }

            int requestID = (int)((DeleteRequest)deleteRequestsDataGrid.SelectedItem).ID;
            try
            {
                _controller.RejectDeleteRequest(requestID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            Refresh();
            MessageBox.Show("Successfully rejected.");
        }

        private void AcceptEditButton_Click(object sender, RoutedEventArgs e)
        {
            if (editRequestsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("You must select an edit request from the table first.");
                return;
            }

            int requestID = (int)((EditRequest)editRequestsDataGrid.SelectedItem).ID;
            try
            {
                _controller.AcceptEditRequest(requestID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            Refresh();
            MessageBox.Show("Successfully accepted.");
        }

        private void RejectEditButton_Click(object sender, RoutedEventArgs e)
        {
            if (editRequestsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("You must select an edit request from the table first.");
                return;
            }

            int requestID = (int)((EditRequest)editRequestsDataGrid.SelectedItem).ID;
            try
            {
                _controller.RejectEditRequest(requestID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            Refresh();
            MessageBox.Show("Successfully rejected.");
        }
    }
}
