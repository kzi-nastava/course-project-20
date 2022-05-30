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
            LoadBlockedPatients();

            HealthRecordRepository.Load();
            VacationRequestRepository.Load();
            UpdateMaxIDsIfNeeded();
        }

        private static void UpdateMaxIDsIfNeeded()
        {
            if (UserService.maxID == -1)
            {
                UserService.CalculateMaxID();
            }
            if (HealthRecordService.maxID == -1)
            {
                HealthRecordService.CalculateMaxID();
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
                Block(selectedPatient);
            }
            else
            {
                MessageBox.Show("Patient is already blocked.");
            }
        }

        private void Block(Patient selectedPatient)
        {
            selectedPatient.IsBlocked = true;
            selectedPatient.BlockedBy = Enums.Blocker.Secretary;
            _blockedPatients.Add(selectedPatient);
            UserRepository.SavePatients();
            patientsDataGrid.Items.Refresh();
            MessageBox.Show("Patient successfully blocked.");
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
                Unblock(selectedPatient);
            }
            else
            {
                MessageBox.Show("Patient is not blocked.");
            }
        }

        private void Unblock(Patient selectedPatient)
        {
            selectedPatient.IsBlocked = false;
            selectedPatient.BlockedBy = Enums.Blocker.None;
            _blockedPatients.Remove(selectedPatient);
            UserRepository.SavePatients();
            patientsDataGrid.Items.Refresh();
            MessageBox.Show("Patient successfully unblocked.");
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            PatientCreateWindow window = new PatientCreateWindow();
            window.ShowDialog();
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

            DeleteHealthRecord(patient);
            HealthRecordRepository.Save();

            DeletePatient(patient);
            UserRepository.SavePatients();

            patientsDataGrid.Items.Refresh();
            MessageBox.Show("Successfully deleted patient and the corresponding health record.");
        }

        private static void DeletePatient(Patient patient)
        {
            UserRepository.Patients.Remove(patient);
            UserRepository.Users.Remove(patient);

            if (patient.ID == UserService.maxID)
            {
                UserService.CalculateMaxID();
            }
        }

        private static void DeleteHealthRecord(Patient patient)
        {
            foreach (HealthRecord record in HealthRecordRepository.Records)
            {
                if (patient.HealthRecordID != record.ID)
                {
                    continue;
                }
                HealthRecordRepository.Records.Remove(record);
                if (patient.HealthRecordID == HealthRecordService.maxID)
                {
                    HealthRecordService.CalculateMaxID();
                }
                break;
            }
        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            if (patientsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("You must select a patient from the table first.");
                return;
            }

            OpenViewWindow();
        }

        private void OpenViewWindow()
        {
            Patient patient = (Patient)patientsDataGrid.SelectedItem;
            HealthRecord record = HealthRecordService.Find(patient);

            PatientViewWindow window = new PatientViewWindow(patient, record);
            window.ShowDialog();
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

            OpenEditWindow();
        }

        private void OpenEditWindow()
        {
            Patient patient = (Patient)patientsDataGrid.SelectedItem;
            HealthRecord record = HealthRecordService.Find(patient);

            PatientEditWindow window = new PatientEditWindow(patient, record);
            window.ShowDialog();
            patientsDataGrid.Items.Refresh();
        }
    }
}
