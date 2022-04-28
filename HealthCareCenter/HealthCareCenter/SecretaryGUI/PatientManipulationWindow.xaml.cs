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
using HealthCareCenter.Service;

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
            foreach (Patient patient in UserRepository.Patients)
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
            patientsDataGrid.ItemsSource = UserRepository.Patients;
            patientsDataGrid.IsReadOnly = true;
            LoadBlockedPatients();
            HealthRecordRepository.Load();
            if (UserService.maxUserID == -1)
            {
                UserService.CalculateMaxUserID();
            }
            if (HealthRecordService.maxHealthRecordID == -1)
            {
                HealthRecordService.CalculateMaxHealthRecordID();
            }
        }

        private void ShowEveryoneRadioButton_Click(object sender, RoutedEventArgs e)
        {
            patientsDataGrid.ItemsSource = UserRepository.Patients;
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
                UserRepository.SavePatients();
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
                UserRepository.SavePatients();
                patientsDataGrid.Items.Refresh();
                MessageBox.Show("Patient successfully unblocked.");
            }
            else
            {
                MessageBox.Show("Patient is not blocked.");
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            PatientCreateWindow patientCreateWindow = new PatientCreateWindow();
            patientCreateWindow.ShowDialog();
            patientsDataGrid.Items.Refresh();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (patientsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("You must select a patient from the table first.");
                return;
            }

            Patient patient = (Patient)patientsDataGrid.SelectedItem;
            
            foreach (HealthRecord record in HealthRecordRepository.HealthRecords)
            {
                if (patient.HealthRecordID == record.ID)
                {
                    HealthRecordRepository.HealthRecords.Remove(record);
                    if (patient.HealthRecordID == HealthRecordService.maxHealthRecordID)
                    {
                        HealthRecordService.CalculateMaxHealthRecordID();
                    }
                    break;
                }
            }
            HealthRecordRepository.Save();

            UserRepository.Patients.Remove(patient);
            UserRepository.Users.Remove(patient);
            UserRepository.SavePatients();

            patientsDataGrid.Items.Refresh();

            if (patient.ID == UserService.maxUserID)
            {
                UserService.CalculateMaxUserID();
            }

            MessageBox.Show("Successfully deleted patient and the corresponding health record.");
        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            if (patientsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("You must select a patient from the table first.");
                return;
            }

            Patient patient = (Patient)patientsDataGrid.SelectedItem;
            HealthRecord record = HealthRecordService.FindRecord(patient);

            PatientViewWindow patientViewWindow = new PatientViewWindow(patient, record);
            patientViewWindow.ShowDialog();
            LoadBlockedPatients();
            patientsDataGrid.Items.Refresh();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (patientsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("You must select a patient from the table first.");
                return;
            }

            Patient patient = (Patient)patientsDataGrid.SelectedItem;
            HealthRecord record = HealthRecordService.FindRecord(patient);

            PatientEditWindow patientEditWindow = new PatientEditWindow(patient, record);
            patientEditWindow.ShowDialog();
            patientsDataGrid.Items.Refresh();
        }
    }
}
