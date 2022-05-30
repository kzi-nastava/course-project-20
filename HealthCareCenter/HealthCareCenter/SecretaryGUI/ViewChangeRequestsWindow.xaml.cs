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
    /// Interaction logic for ViewChangeRequestsWindow.xaml
    /// </summary>
    public partial class ViewChangeRequestsWindow : Window
    {
        private Patient _patient;

        private List<DeleteRequest> _deleteRequests;
        private List<EditRequest> _editRequests;

        public ViewChangeRequestsWindow()
        {
            InitializeComponent();
        }

        public ViewChangeRequestsWindow(Patient patient)
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
            _deleteRequests = new List<DeleteRequest>();
            _editRequests = new List<EditRequest>();
            foreach (AppointmentChangeRequest request in AppointmentChangeRequestRepository.Requests)
            {
                if (request.State != RequestState.Waiting || request.PatientID != _patient.ID)
                {
                    continue;
                }
                if (request.RequestType == RequestType.Delete)
                {
                    AddDeleteRequest(request);
                }
                else
                {
                    AddEditRequest(request);
                }
            }
            deleteRequestsDataGrid.ItemsSource = _deleteRequests;
            editRequestsDataGrid.ItemsSource = _editRequests;
        }

        private void AddEditRequest(AppointmentChangeRequest request)
        {
            EditRequest editRequest = new EditRequest(request.ID, request.DateSent);

            foreach (Appointment appointment in AppointmentRepository.Appointments)
            {
                if (appointment.ID != request.AppointmentID)
                {
                    continue;
                }

                LinkDoctorEdit(request, editRequest, appointment);

                editRequest.OriginalAppointmentTime = appointment.ScheduledDate;
                editRequest.OriginalType = appointment.Type;
                break;
            }
            editRequest.NewAppointmentTime = request.NewDate;
            editRequest.NewType = request.NewAppointmentType;
            _editRequests.Add(editRequest);
        }

        private static void LinkDoctorEdit(AppointmentChangeRequest request, EditRequest editRequest, Appointment appointment)
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
                    return;
                }
            }
        }

        private void AddDeleteRequest(AppointmentChangeRequest request)
        {
            DeleteRequest deleteRequest = new DeleteRequest(request.ID, request.DateSent);

            foreach (Appointment appointment in AppointmentRepository.Appointments)
            {
                if (appointment.ID != request.AppointmentID)
                {
                    continue;
                }

                LinkDoctorDelete(deleteRequest, appointment);
                deleteRequest.AppointmentTime = appointment.ScheduledDate;
                break;
            }
            _deleteRequests.Add(deleteRequest);
        }

        private static void LinkDoctorDelete(DeleteRequest deleteRequest, Appointment appointment)
        {
            foreach (Doctor doctor in UserRepository.Doctors)
            {
                if (doctor.ID == appointment.DoctorID)
                {
                    deleteRequest.DoctorUsername = doctor.Username;
                    return;
                }
            }
        }

        private void AcceptDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (deleteRequestsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("You must select a delete request from the table first.");
                return;
            }

            AcceptDeleteRequest();
            Refresh();
            MessageBox.Show("Successfully accepted.");
        }

        private void AcceptDeleteRequest()
        {
            int requestID = (int)((DeleteRequest)deleteRequestsDataGrid.SelectedItem).ID;

            AppointmentChangeRequest request = FindRequest(requestID);
            request.State = RequestState.Approved;
            AppointmentChangeRequestService.DeleteAppointment(request);

            AppointmentRepository.Save();
            AppointmentChangeRequestRepository.Save();
        }

        private void RejectDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (deleteRequestsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("You must select a delete request from the table first.");
                return;
            }

            RejectDeleteRequest();
            Refresh();
            MessageBox.Show("Successfully rejected.");
        }

        private void RejectDeleteRequest()
        {
            int requestID = (int)((DeleteRequest)deleteRequestsDataGrid.SelectedItem).ID;
            FindRequest(requestID).State = RequestState.Denied;
            AppointmentChangeRequestRepository.Save();
        }

        private void AcceptEditButton_Click(object sender, RoutedEventArgs e)
        {
            if (editRequestsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("You must select an edit request from the table first.");
                return;
            }

            AcceptEditRequest();
            Refresh();
            MessageBox.Show("Successfully accepted.");
        }

        private void AcceptEditRequest()
        {
            int requestID = (int)((EditRequest)editRequestsDataGrid.SelectedItem).ID;
            AppointmentChangeRequest request = FindRequest(requestID);
            request.State = RequestState.Approved;
            AppointmentChangeRequestService.EditAppointment(request);

            AppointmentRepository.Save();
            AppointmentChangeRequestRepository.Save();
        }

        private void RejectEditButton_Click(object sender, RoutedEventArgs e)
        {
            if (editRequestsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("You must select an edit request from the table first.");
                return;
            }

            RejectEditRequest();
            Refresh();
            MessageBox.Show("Successfully rejected.");
        }

        private void RejectEditRequest()
        {
            int requestID = (int)((EditRequest)editRequestsDataGrid.SelectedItem).ID;
            FindRequest(requestID).State = RequestState.Denied;
            AppointmentChangeRequestRepository.Save();
        }

        private static AppointmentChangeRequest FindRequest(int requestID)
        {
            foreach (AppointmentChangeRequest request in AppointmentChangeRequestRepository.Requests)
            {
                if (request.ID == requestID)
                {
                    return request;
                }
            }
            return null;
        }
    }
}
