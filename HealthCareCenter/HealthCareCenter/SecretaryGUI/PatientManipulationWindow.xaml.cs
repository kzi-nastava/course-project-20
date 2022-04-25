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
    /// Interaction logic for PatientManipulationWindow.xaml
    /// </summary>
    public partial class PatientManipulationWindow : Window
    {
        private List<Patient> _blockedPatients;

        private void LoadBlockedPatients()
        {
            _blockedPatients = new List<Patient>();
            foreach (Patient patient in UserManager.Patients)
            {
                if (patient.IsBlocked)
                {
                    _blockedPatients.Add(patient);
                }
            }
        }

        public PatientManipulationWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            patientsDataGrid.ItemsSource = UserManager.Patients;
            patientsDataGrid.IsReadOnly = true;
            LoadBlockedPatients();
            HealthRecordManager.LoadHealthRecords();
        }

        private void ShowEveryoneRadioButton_Click(object sender, RoutedEventArgs e)
        {
            patientsDataGrid.ItemsSource = UserManager.Patients;
        }

        private void ShowOnlyBlockedRadioButton_Click(object sender, RoutedEventArgs e)
        {
            patientsDataGrid.ItemsSource = _blockedPatients;
        }

        private void BlockButton_Click(object sender, RoutedEventArgs e)
        {
            if (patientsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("You must select a patient from the table first.");
                return;
            }

            Patient selectedPatient = (Patient)patientsDataGrid.SelectedItem;
            if (!selectedPatient.IsBlocked)
            {
                selectedPatient.IsBlocked = true;
                selectedPatient.BlockedBy = Enums.Blocker.Secretary;
                _blockedPatients.Add(selectedPatient);
                UserManager.SavePatients();
                patientsDataGrid.Items.Refresh();
                MessageBox.Show("Patient successfully blocked.");
            }
            else
            {
                MessageBox.Show("Patient is already blocked.");
            }
        }

        private void UnblockButton_Click(object sender, RoutedEventArgs e)
        {
            if (patientsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("You must select a patient from the table first.");
                return;
            }

            Patient selectedPatient = (Patient)patientsDataGrid.SelectedItem;
            if (selectedPatient.IsBlocked)
            {
                selectedPatient.IsBlocked = false;
                selectedPatient.BlockedBy = Enums.Blocker.None;
                _blockedPatients.Remove(selectedPatient);
                UserManager.SavePatients();
                patientsDataGrid.Items.Refresh();
                MessageBox.Show("Patient successfully unblocked.");
            }
            else
            {
                MessageBox.Show("Patient is not blocked.");
            }
        }
    }
}
