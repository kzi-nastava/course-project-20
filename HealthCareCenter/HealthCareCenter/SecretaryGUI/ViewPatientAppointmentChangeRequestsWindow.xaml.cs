using HealthCareCenter.Enums;
using HealthCareCenter.Model;
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

namespace HealthCareCenter.SecretaryGUI
{
    /// <summary>
    /// Interaction logic for ViewPatientAppointmentChangeRequestsWindow.xaml
    /// </summary>
    public partial class ViewPatientAppointmentChangeRequestsWindow : Window
    {
        private Patient _patient;
        private DataTable _editRequestsTable;
        private DataTable _deleteRequestsTable;

        public ViewPatientAppointmentChangeRequestsWindow()
        {
            InitializeComponent();
        }

        public ViewPatientAppointmentChangeRequestsWindow(Patient patient)
        {
            this._patient = patient;
            InitializeComponent();
        }

        private void CreateDeleteRequestsTable()
        {
            _deleteRequestsTable = new DataTable("Delete requests");
            _deleteRequestsTable.Columns.Add(new DataColumn("ID", typeof(int)));
            _deleteRequestsTable.Columns.Add(new DataColumn("Date sent", typeof(DateTime)));
            _deleteRequestsTable.Columns.Add(new DataColumn("Doctor", typeof(string)));
            _deleteRequestsTable.Columns.Add(new DataColumn("App. time", typeof(DateTime)));
        }

        private void CreateEditRequestsTable()
        {
            _editRequestsTable = new DataTable("Edit requests");
            _editRequestsTable.Columns.Add(new DataColumn("ID", typeof(int)));
            _editRequestsTable.Columns.Add(new DataColumn("Date sent", typeof(DateTime)));
            _editRequestsTable.Columns.Add(new DataColumn("Orig. doctor", typeof(string)));
            _editRequestsTable.Columns.Add(new DataColumn("New doctor", typeof(string)));
            _editRequestsTable.Columns.Add(new DataColumn("Orig. App. time", typeof(DateTime)));
            _editRequestsTable.Columns.Add(new DataColumn("New App. time", typeof(DateTime)));
            _editRequestsTable.Columns.Add(new DataColumn("Orig. App. type", typeof(AppointmentType)));
            _editRequestsTable.Columns.Add(new DataColumn("New App. type", typeof(AppointmentType)));
        }

        private void Refresh()
        {
            CreateDeleteRequestsTable();
            CreateEditRequestsTable();
            foreach (AppointmentChangeRequest request in AppointmentChangeRequestRepository.AllChangeRequests)
            {
                if (request.RequestState == Enums.RequestState.Waiting && request.PatientID == _patient.ID)
                {
                    if (request.RequestType == Enums.RequestType.Delete)
                    {
                        DataRow row = _deleteRequestsTable.NewRow();
                        row[0] = request.ID;
                        row[1] = request.DateSent;
                        foreach (Appointment appointment in AppointmentRepository.AllAppointments)
                        {
                            if (appointment.ID == request.AppointmentID)
                            {
                                foreach (Doctor doctor in UserRepository.Doctors)
                                {
                                    if (doctor.ID == appointment.DoctorID)
                                    {
                                        row[2] = doctor.Username;
                                        break;
                                    }
                                }
                                row[3] = appointment.AppointmentDate;
                                break;
                            }
                        }
                        _deleteRequestsTable.Rows.Add(row);
                    }
                    else
                    {
                        DataRow row = _editRequestsTable.NewRow();
                        row[0] = request.ID;
                        row[1] = request.DateSent;
                        foreach (Appointment appointment in AppointmentRepository.AllAppointments)
                        {
                            if (appointment.ID == request.AppointmentID)
                            {
                                bool foundOld = false;
                                bool foundNew = false;
                                foreach (Doctor doctor in UserRepository.Doctors)
                                {
                                    if (doctor.ID == appointment.DoctorID)
                                    {
                                        row[2] = doctor.Username;
                                        foundOld = true;
                                    }
                                    else if (doctor.ID == request.NewDoctorID)
                                    {
                                        row[3] = doctor.Username;
                                        foundNew = true;
                                    }
                                    if (foundNew && foundOld)
                                    {
                                        break;
                                    }
                                }
                                row[4] = appointment.AppointmentDate;
                                row[6] = appointment.Type;
                                break;
                            }
                        }
                        row[5] = request.NewDate;
                        row[7] = request.NewAppointmentType;
                        _editRequestsTable.Rows.Add(row);
                    }
                }
            }
            deleteRequestsDataGrid.ItemsSource = _deleteRequestsTable.DefaultView;
            editRequestsDataGrid.ItemsSource = _editRequestsTable.DefaultView;
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            AppointmentChangeRequestRepository.Load();
            AppointmentRepository.Load();

            Refresh();

            deleteRequestsDataGrid.IsReadOnly = true;
            editRequestsDataGrid.IsReadOnly = true;
        }

        private void AcceptDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (deleteRequestsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("You must select a delete request from the table first.");
                return;
            }

            int requestID = (int)((DataRowView)deleteRequestsDataGrid.SelectedItem).Row.ItemArray[0];

            foreach (AppointmentChangeRequest request in AppointmentChangeRequestRepository.AllChangeRequests)
            {
                
                if (request.ID == requestID)
                {
                    request.RequestState = RequestState.Approved;
                    ChangeRequestService.DeleteAppointment(request);
                    break;
                }
            }

            AppointmentRepository.Save();
            AppointmentChangeRequestRepository.Save();
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

            int requestID = (int)((DataRowView)deleteRequestsDataGrid.SelectedItem).Row.ItemArray[0];

            foreach (AppointmentChangeRequest request in AppointmentChangeRequestRepository.AllChangeRequests)
            {

                if (request.ID == requestID)
                {
                    request.RequestState = RequestState.Denied;
                    break;
                }
            }

            AppointmentChangeRequestRepository.Save();
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

            int requestID = (int)((DataRowView)editRequestsDataGrid.SelectedItem).Row.ItemArray[0];

            foreach (AppointmentChangeRequest request in AppointmentChangeRequestRepository.AllChangeRequests)
            {

                if (request.ID == requestID)
                {
                    request.RequestState = RequestState.Approved;
                    ChangeRequestService.MakeChangesToAppointment(request);
                    break;
                }
            }

            AppointmentRepository.Save();
            AppointmentChangeRequestRepository.Save();
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

            int requestID = (int)((DataRowView)editRequestsDataGrid.SelectedItem).Row.ItemArray[0];

            foreach (AppointmentChangeRequest request in AppointmentChangeRequestRepository.AllChangeRequests)
            {

                if (request.ID == requestID)
                {
                    request.RequestState = RequestState.Denied;
                    break;
                }
            }

            AppointmentChangeRequestRepository.Save();
            Refresh();
            MessageBox.Show("Successfully rejected.");
        }
    }
}
