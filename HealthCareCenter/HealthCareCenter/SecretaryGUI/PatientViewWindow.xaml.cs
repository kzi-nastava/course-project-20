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
using HealthCareCenter.Model;

namespace HealthCareCenter.SecretaryGUI
{
    /// <summary>
    /// Interaction logic for PatientViewWindow.xaml
    /// </summary>
    public partial class PatientViewWindow : Window
    {
        private Patient _patient;
        private HealthRecord _record;

        public PatientViewWindow()
        {
            InitializeComponent();
        }

        public PatientViewWindow(Patient patient, HealthRecord record)
        {
            this._patient = patient;
            this._record = record;
            InitializeComponent();
        }

        private void InitializeWindow()
        {
            idTextBox.Text = _patient.ID.ToString();
            firstNameTextBox.Text = _patient.FirstName;
            usernameTextBox.Text = _patient.Username;
            lastNameTextBox.Text = _patient.LastName;
            passwordTextBox.Text = _patient.Password;
            birthDatePicker.SelectedDate = _patient.DateOfBirth;
            isBlockedCheckBox.IsChecked = _patient.IsBlocked;
            blockedByTextBox.Text = _patient.BlockedBy.ToString();
            heightTextBox.Text = _record.Height.ToString();
            weightTextBox.Text = _record.Weight.ToString();
            previousDiseasesListBox.ItemsSource = _record.PreviousDiseases;
            allergensListBox.ItemsSource = _record.Allergens;
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            InitializeWindow();
        }

        private void BlockButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_patient.IsBlocked)
            {
                _patient.IsBlocked = true;
                _patient.BlockedBy = Enums.Blocker.Secretary;
                UserRepository.SavePatients();
                isBlockedCheckBox.IsChecked = true;
                blockedByTextBox.Text = Enums.Blocker.Secretary.ToString();
                MessageBox.Show("Patient successfully blocked.");
            }
            else
            {
                MessageBox.Show("Patient is already blocked.");
            }
        }

        private void UnblockButton_Click(object sender, RoutedEventArgs e)
        {
            if (_patient.IsBlocked)
            {
                _patient.IsBlocked = false;
                _patient.BlockedBy = Enums.Blocker.None;
                UserRepository.SavePatients();
                isBlockedCheckBox.IsChecked = false;
                blockedByTextBox.Text = Enums.Blocker.None.ToString();
                MessageBox.Show("Patient successfully unblocked.");
            }
            else
            {
                MessageBox.Show("Patient is not blocked.");
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            PatientEditWindow window = new PatientEditWindow(_patient, _record);
            window.ShowDialog();
            InitializeWindow();
            previousDiseasesListBox.Items.Refresh();
            allergensListBox.Items.Refresh();
        }

        private void ViewChangeAppointmentRequestsButton_Click(object sender, RoutedEventArgs e)
        {
            ViewPatientAppointmentChangeRequestsWindow window = new ViewPatientAppointmentChangeRequestsWindow(_patient);
            window.ShowDialog();
        }
    }
}
