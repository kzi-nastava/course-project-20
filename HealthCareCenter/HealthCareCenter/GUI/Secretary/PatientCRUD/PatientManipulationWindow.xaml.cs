﻿using System;
using System.Collections.Generic;
using System.Windows;
using HealthCareCenter.Core.HealthRecords;
using HealthCareCenter.Core.Patients;
using HealthCareCenter.Core.Patients.Controllers;
using HealthCareCenter.Core.Users;

namespace HealthCareCenter.Secretary
{
    /// <summary>
    /// Interaction logic for PatientManipulationWindow.xaml
    /// </summary>
    public partial class PatientManipulationWindow : Window
    {
        private List<Patient> _blockedPatients;
        private PatientManipulationController _controller;

        public PatientManipulationWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            _controller = new PatientManipulationController();

            patientsDataGrid.ItemsSource = UserRepository.Patients;
            try
            {
                _blockedPatients = _controller.GetBlockedPatients();
                _controller.UpdateMaxIDsIfNeeded();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
            try
            {
                _controller.Block(selectedPatient, _blockedPatients);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            
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
            try
            {
                _controller.Unblock(selectedPatient, _blockedPatients);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

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
            try
            {
                _controller.Delete(patient);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            patientsDataGrid.Items.Refresh();
            MessageBox.Show("Successfully deleted patient and the corresponding health record.");
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
            HealthRecord record;
            try
            {
                record = _controller.GetRecord(patient);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            
            PatientViewWindow window = new PatientViewWindow(patient, record);
            window.ShowDialog();
            try
            {
                _blockedPatients = _controller.GetBlockedPatients();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
            HealthRecord record;
            try
            {
                record = _controller.GetRecord(patient);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            PatientEditWindow window = new PatientEditWindow(patient, record);
            window.ShowDialog();
            patientsDataGrid.Items.Refresh();
        }
    }
}