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

namespace HealthCareCenter.SecretaryGUI
{
    /// <summary>
    /// Interaction logic for VacationRequestsWindow.xaml
    /// </summary>
    public partial class VacationRequestsWindow : Window
    {
        private List<VacationRequestDisplay> _vacationRequests;

        private void Refresh()
        {
            _vacationRequests = new List<VacationRequestDisplay>();

            foreach (VacationRequest request in VacationRequestRepository.Requests)
            {
                if (request.State != RequestState.Waiting || request.StartDate.CompareTo(DateTime.Now) <= 0)
                    continue;
                VacationRequestDisplay requestDisplay = new VacationRequestDisplay()
                {
                    ID = request.ID,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    RequestReason = request.RequestReason
                };
                LinkDoctor(request, requestDisplay);
                _vacationRequests.Add(requestDisplay);
            }

            vacationRequestsDataGrid.ItemsSource = _vacationRequests;
        }

        private static void LinkDoctor(VacationRequest request, VacationRequestDisplay requestDisplay)
        {
            foreach (Doctor doctor in UserRepository.Doctors)
            {
                if (doctor.ID == request.DoctorID)
                {
                    requestDisplay.DoctorName = doctor.FirstName + " " + doctor.LastName;
                    return;
                }
            }
        }

        public VacationRequestsWindow()
        {
            VacationRequestRepository.Load();

            InitializeComponent();

            Refresh();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (vacationRequestsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("You must select a vacation request first.");
                return;
            }

            int id = ((VacationRequestDisplay)vacationRequestsDataGrid.SelectedItem).ID;
            AcceptRequest(id);
            Refresh();
            MessageBox.Show("Successfully accepted the vacation request.");
        }

        private void AcceptRequest(int id)
        {
            foreach (VacationRequest request in VacationRequestRepository.Requests)
            {
                if (request.ID == id)
                {
                    request.State = RequestState.Approved;
                    break;
                }
            }
            VacationRequestRepository.Save();
        }

        private void DenyButton_Click(object sender, RoutedEventArgs e)
        {
            if (vacationRequestsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("You must select a vacation request first.");
                return;
            }
            if (string.IsNullOrWhiteSpace(denialReasonTextBox.Text))
            {
                MessageBox.Show("You must enter the reason for denial first.");
                return;
            }

            int id = ((VacationRequestDisplay)vacationRequestsDataGrid.SelectedItem).ID;
            DenyRequest(id, denialReasonTextBox.Text);
            Refresh();
            denialReasonTextBox.Clear();
            MessageBox.Show("Successfully denied the vacation request.");
        }

        private static void DenyRequest(int id, string reason)
        {
            foreach (VacationRequest request in VacationRequestRepository.Requests)
            {
                if (request.ID == id)
                {
                    request.State = RequestState.Denied;
                    request.DenialReason = reason;
                    break;
                }
            }
            VacationRequestRepository.Save();
        }
    }
}
