using HealthCareCenter.Core.HealthRecords;
using HealthCareCenter.Core.Patients;
using HealthCareCenter.Core.Patients.Controllers;
using HealthCareCenter.Core.Patients.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HealthCareCenter.Secretary
{
    /// <summary>
    /// Interaction logic for PatientEditWindow.xaml
    /// </summary>
    public partial class PatientEditWindow : Window
    {
        private readonly Patient _patient;
        private readonly HealthRecord _record;

        private ObservableCollection<string> _previousDiseases;
        private ObservableCollection<string> _allergens;

        private readonly PatientEditController _controller;

        public PatientEditWindow()
        {
            InitializeComponent();
        }

        public PatientEditWindow(Patient patient, HealthRecord record)
        {
            _patient = patient;
            _record = record;
            InitializeComponent();

            _controller = new PatientEditController();
        }

        private void Reset()
        {
            firstNameTextBox.Clear();
            usernameTextBox.Clear();
            lastNameTextBox.Clear();
            passwordTextBox.Clear();
            heightTextBox.Clear();
            weightTextBox.Clear();
            previousDiseaseTextBox.Clear();
            allergenTextBox.Clear();
            previousDiseasesListBox.ItemsSource = null;
            allergensListBox.ItemsSource = null;
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        private void AddPreviousDiseaseButton_Click(object sender, RoutedEventArgs e)
        {
            string disease;
            try
            {
                disease = _controller.ValidatePreviousDisease(previousDiseaseTextBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            _previousDiseases.Add(previousDiseaseTextBox.Text);
        }

        private void AddAllergenButton_Click(object sender, RoutedEventArgs e)
        {
            string allergen;
            try
            {
                allergen = _controller.ValidateAllergen(allergenTextBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            _allergens.Add(allergenTextBox.Text);
        }

        private void DeletePreviousDiseaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (previousDiseasesListBox.SelectedItem != null)
            {
                _previousDiseases.Remove(previousDiseasesListBox.SelectedItem.ToString());
            }
            else
            {
                MessageBox.Show("You need to select a disease from the list first.");
            }
        }

        private void DeleteAllergenButton_Click(object sender, RoutedEventArgs e)
        {
            if (allergensListBox.SelectedItem != null)
            {
                _allergens.Remove(allergensListBox.SelectedItem.ToString());
            }
            else
            {
                MessageBox.Show("You need to select an allergen from the list first.");
            }
        }

        private void EditPreviousDiseaseButton_Click(object sender, RoutedEventArgs e)
        {
            string disease;
            try
            {
                disease = _controller.ValidatePreviousDisease(previousDiseaseTextBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            if (previousDiseasesListBox.SelectedItem != null)
            {
                _previousDiseases[previousDiseasesListBox.SelectedIndex] = previousDiseaseTextBox.Text;
            }
            else
            {
                MessageBox.Show("You need to select a disease from the list first.");
            }
        }

        private void EditAllergenButton_Click(object sender, RoutedEventArgs e)
        {
            string allergen;
            try
            {
                allergen = _controller.ValidateAllergen(allergenTextBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            if (allergensListBox.SelectedItem != null)
            {
                _allergens[allergensListBox.SelectedIndex] = allergenTextBox.Text;
            }
            else
            {
                MessageBox.Show("You need to select an allergen from the list first.");
            }
        }

        private void PreviousDiseasesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (previousDiseasesListBox.SelectedItem != null)
            {
                previousDiseaseTextBox.Text = previousDiseasesListBox.SelectedValue.ToString();
            }
            else
            {
                previousDiseaseTextBox.Clear();
            }
        }

        private void AllergensListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (allergensListBox.SelectedItem != null)
            {
                allergenTextBox.Text = allergensListBox.SelectedValue.ToString();
            }
            else
            {
                allergenTextBox.Clear();
            }
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            idTextBox.Text = _patient.ID.ToString();
            firstNameTextBox.Text = _patient.FirstName;
            usernameTextBox.Text = _patient.Username;
            lastNameTextBox.Text = _patient.LastName;
            passwordTextBox.Text = _patient.Password;
            birthDatePicker.SelectedDate = _patient.DateOfBirth;
            heightTextBox.Text = _record.Height.ToString();
            weightTextBox.Text = _record.Weight.ToString();
            _previousDiseases = new ObservableCollection<string>(_record.PreviousDiseases);
            previousDiseasesListBox.ItemsSource = _previousDiseases;
            _allergens = new ObservableCollection<string>(_record.Allergens);
            allergensListBox.ItemsSource = _allergens;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            HealthRecordDTO editedRecord = new HealthRecordDTO(_record.ID, heightTextBox.Text, weightTextBox.Text, _previousDiseases.Cast<String>().ToList(), _allergens.Cast<String>().ToList(), _record.PatientID);
            PatientDTO editedPatient = new PatientDTO(_patient.ID, usernameTextBox.Text, passwordTextBox.Text, firstNameTextBox.Text, lastNameTextBox.Text, birthDatePicker.SelectedDate, _patient.IsBlocked, _patient.BlockedBy, _patient.PrescriptionIDs, _patient.HealthRecordID);

            try
            {
                _controller.Edit(editedPatient, editedRecord, _patient, _record);
            } 
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            MessageBox.Show("Successfully edited the patient and health record.");
        }
    }
}
