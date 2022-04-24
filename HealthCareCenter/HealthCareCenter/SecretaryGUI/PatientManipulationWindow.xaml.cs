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
        }

        private void ShowEveryoneRadioButton_Click(object sender, RoutedEventArgs e)
        {
            patientsDataGrid.ItemsSource = UserManager.Patients;
        }

        private void ShowOnlyBlockedRadioButton_Click(object sender, RoutedEventArgs e)
        {
            patientsDataGrid.ItemsSource = _blockedPatients;
        }
    }
}
