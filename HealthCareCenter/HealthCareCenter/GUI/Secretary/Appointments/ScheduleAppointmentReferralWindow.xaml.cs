using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Rooms.Models;
using HealthCareCenter.Core.Referrals.Services;
using HealthCareCenter.Core;
using HealthCareCenter.Core.Referrals.Models;
using HealthCareCenter.Core.VacationRequests.Services;
using HealthCareCenter.Core.Notifications.Services;
using HealthCareCenter.Core.Notifications.Repositories;
using HealthCareCenter.Core.VacationRequests.Repositories;
using HealthCareCenter.Core.Appointments.Services;
using HealthCareCenter.Core.Referrals.Controllers;
using HealthCareCenter.Core.Patients;

namespace HealthCareCenter.Secretary
{
    /// <summary>
    /// Interaction logic for ScheduleAppointmentReferralWindow.xaml
    /// </summary>
    public partial class ScheduleAppointmentReferralWindow : Window
    {
        private readonly Patient _patient;
        private readonly Referral _referral;
        private readonly ScheduleAppointmentReferralController _controller;

        public ScheduleAppointmentReferralWindow()
        {
            InitializeComponent();
        }

        public ScheduleAppointmentReferralWindow(Patient patient, Referral referral, IReferralsService referralsService)
        {
            _patient = patient;
            _referral = referral;

            _controller = new ScheduleAppointmentReferralController(new VacationRequestService(new NotificationService(new NotificationRepository()), new VacationRequestRepository()), new TermsService(), referralsService);

            InitializeComponent();

            Refresh();
        }

        private void Refresh()
        {
            RefreshTerms();
            RefreshRooms();
        }

        private void RefreshTerms()
        {
            if (termDatePicker.SelectedDate == null)
            {
                return;
            }

            try
            {
                termsListBox.ItemsSource = _controller.GetAvailableTerms(_referral.DoctorID, (DateTime)termDatePicker.SelectedDate);
            }
            catch (Exception ex)
            {
                termsListBox.ItemsSource = new List<string>();
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void RefreshRooms()
        {
            if (checkupRadioButton == null || operationRadioButton == null)
            {
                return;
            }

            try
            {
                roomsDataGrid.ItemsSource = _controller.GetRooms((bool)checkupRadioButton.IsChecked);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void AppointmentTypeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RefreshRooms();
        }

        private void TermDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshTerms();
        }

        private void ScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            if (!EnteredData())
                return;

            DateTime scheduledDate = GetScheduledDate();
            int roomID = ((HospitalRoomForDisplay)roomsDataGrid.SelectedItem).ID;

            Appointment appointment = new Appointment(scheduledDate, roomID, _referral.DoctorID, _patient.HealthRecordID, SelectedAppointmentType(), false);

            try
            {
                _controller.Schedule(_referral, appointment);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            MessageBox.Show("Successfully scheduled appointment via referral.");
            Close();
        }

        private AppointmentType SelectedAppointmentType()
        {
            if ((bool)checkupRadioButton.IsChecked)
                return AppointmentType.Checkup;
            else
                return AppointmentType.Operation;
        }

        private DateTime GetScheduledDate()
        {
            string[] scheduledHoursMinutes = termsListBox.SelectedItem.ToString().Split(":");
            int hours = int.Parse(scheduledHoursMinutes[0]);
            int minutes = int.Parse(scheduledHoursMinutes[1]);
            DateTime scheduledDate = ((DateTime)termDatePicker.SelectedDate).Date.AddHours(hours).AddMinutes(minutes);
            return scheduledDate;
        }

        private bool EnteredData()
        {
            if (roomsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("You must select a room first.");
                return false;
            }
            if (termsListBox.SelectedItem == null)
            {
                MessageBox.Show("You must select a term first.");
                return false;
            }
            if (!(bool)checkupRadioButton.IsChecked && !(bool)operationRadioButton.IsChecked)
            {
                MessageBox.Show("You must select an appointment type first.");
                return false;
            }
            if (termDatePicker.SelectedDate == null)
            {
                MessageBox.Show("You must select a term date first.");
                return false;
            }
            return true;
        }
    }
}
