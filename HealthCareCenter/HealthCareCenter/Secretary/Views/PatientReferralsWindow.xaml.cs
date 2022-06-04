using HealthCareCenter.Model;
using HealthCareCenter.Secretary.Controllers;
using System;
using System.Collections.Generic;
using System.Windows;

namespace HealthCareCenter.Secretary
{
    /// <summary>
    /// Interaction logic for PatientReferralsWindow.xaml
    /// </summary>
    public partial class PatientReferralsWindow : Window
    {
        private readonly Patient _patient;
        private List<PatientReferralForDisplay> _referrals;

        private readonly PatientReferralsController _controller;

        public PatientReferralsWindow()
        {
            InitializeComponent();
        }

        public PatientReferralsWindow(Patient patient)
        {
            _patient = patient;
            _controller = new PatientReferralsController();

            InitializeComponent();

            Refresh();
        }

        private void Refresh()
        {
            try
            {
                _referrals = _controller.Get(_patient);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            referralsDataGrid.ItemsSource = _referrals;
        }

        private void UseReferralButton_Click(object sender, RoutedEventArgs e)
        {
            if (referralsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("You must first select a referral from the table to use!");
                return;
            }

            Referral selectedReferral;
            try
            {
                selectedReferral = _controller.Get(((PatientReferralForDisplay)referralsDataGrid.SelectedItem).ID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            ScheduleAppointmentReferralWindow window = new ScheduleAppointmentReferralWindow(_patient, selectedReferral);
            window.ShowDialog();
            Refresh();
        }
    }
}
