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

        private List<DeleteAppointmentChangeRequestDisplay> _deleteRequests;
        private List<EditAppointmentChangeRequestDisplay> _editRequests;

        public ViewPatientAppointmentChangeRequestsWindow()
        {
            InitializeComponent();
        }

        public ViewPatientAppointmentChangeRequestsWindow(Patient patient)
        {
            this._patient = patient;
            AppointmentChangeRequestRepository.Load();
            AppointmentRepository.Load();

            InitializeComponent();

            deleteRequestsDataGrid.IsReadOnly = true;
            editRequestsDataGrid.IsReadOnly = true;
            Refresh();
        }

        private void Refresh()
        {
            _deleteRequests = new List<DeleteAppointmentChangeRequestDisplay>();
            _editRequests = new List<EditAppointmentChangeRequestDisplay>();
            foreach (AppointmentChangeRequest request in AppointmentChangeRequestRepository.Requests)
            {
                if (request.State == Enums.RequestState.Waiting && request.PatientID == _patient.ID)
                {
                    if (request.RequestType == Enums.RequestType.Delete)
                    {
                        DeleteAppointmentChangeRequestDisplay deleteRequest = new DeleteAppointmentChangeRequestDisplay
                        {
                            ID = request.ID,
                            TimeSent = request.DateSent
                        };

                        foreach (Appointment appointment in AppointmentRepository.Appointments)
                        {
                            if (appointment.ID == request.AppointmentID)
                            {
                                foreach (Doctor doctor in UserRepository.Doctors)
                                {
                                    if (doctor.ID == appointment.DoctorID)
                                    {
                                        deleteRequest.DoctorUsername = doctor.Username;
                                        break;
                                    }
                                }
                                deleteRequest.AppointmentTime = appointment.ScheduledDate;
                                break;
                            }
                        }
                        _deleteRequests.Add(deleteRequest);
                    }
                    else
                    {
                        EditAppointmentChangeRequestDisplay editRequest = new EditAppointmentChangeRequestDisplay
                        {
                            ID = request.ID,
                            TimeSent = request.DateSent
                        };

                        foreach (Appointment appointment in AppointmentRepository.Appointments)
                        {
                            if (appointment.ID == request.AppointmentID)
                            {
                                bool foundOld = false;
                                bool foundNew = false;
                                foreach (Doctor doctor in UserRepository.Doctors)
                                {
                                    if (doctor.ID == appointment.DoctorID)
                                    {
                                        editRequest.OriginalDoctorUsername = doctor.Username;
                                        foundOld = true;
                                    }
                                    if (doctor.ID == request.NewDoctorID)
                                    {
                                        editRequest.NewDoctorUsername = doctor.Username;
                                        foundNew = true;
                                    }
                                    if (foundNew && foundOld)
                                    {
                                        break;
                                    }
                                }
                                editRequest.OriginalAppointmentTime = appointment.ScheduledDate;
                                editRequest.OriginalAppointmentType = appointment.Type;
                                break;
                            }
                        }
                        editRequest.NewAppointmentTime = request.NewDate;
                        editRequest.NewAppointmentType = request.NewAppointmentType;
                        _editRequests.Add(editRequest);
                    }
                }
            }
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

            int requestID = (int)((DeleteAppointmentChangeRequestDisplay)deleteRequestsDataGrid.SelectedItem).ID;

            foreach (AppointmentChangeRequest request in AppointmentChangeRequestRepository.Requests)
            {

                if (request.ID == requestID)
                {
                    request.State = RequestState.Approved;
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

            int requestID = (int)((DeleteAppointmentChangeRequestDisplay)deleteRequestsDataGrid.SelectedItem).ID;

            foreach (AppointmentChangeRequest request in AppointmentChangeRequestRepository.Requests)
            {

                if (request.ID == requestID)
                {
                    request.State = RequestState.Denied;
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

            int requestID = (int)((EditAppointmentChangeRequestDisplay)editRequestsDataGrid.SelectedItem).ID;

            foreach (AppointmentChangeRequest request in AppointmentChangeRequestRepository.Requests)
            {

                if (request.ID == requestID)
                {
                    request.State = RequestState.Approved;
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

            int requestID = (int)((EditAppointmentChangeRequestDisplay)editRequestsDataGrid.SelectedItem).ID;

            foreach (AppointmentChangeRequest request in AppointmentChangeRequestRepository.Requests)
            {

                if (request.ID == requestID)
                {
                    request.State = RequestState.Denied;
                    break;
                }
            }

            AppointmentChangeRequestRepository.Save();
            Refresh();
            MessageBox.Show("Successfully rejected.");
        }
    }
}
