using HealthCareCenter.Model;
using HealthCareCenter.Service;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for PatientCreateWindow.xaml
    /// </summary>
    public partial class PatientCreateWindow : Window
    {
        public PatientCreateWindow()
        {
            InitializeComponent();
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
            previousDiseasesListBox.Items.Clear();
            allergensListBox.Items.Clear();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        private void AddPreviousDiseaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(previousDiseaseTextBox.Text))
            {
                MessageBox.Show("You must enter a disease.");
                return;
            }
            previousDiseasesListBox.Items.Add(previousDiseaseTextBox.Text);
        }

        private void AddAllergenButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(allergenTextBox.Text))
            {
                MessageBox.Show("You must enter an allergen.");
                return;
            }
            allergensListBox.Items.Add(allergenTextBox.Text);
        }

        private void DeletePreviousDiseaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (previousDiseasesListBox.SelectedItem != null) 
            {
                previousDiseasesListBox.Items.Remove(previousDiseasesListBox.SelectedItem);
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
                allergensListBox.Items.Remove(allergensListBox.SelectedItem);
            }
            else
            {
                MessageBox.Show("You need to select an allergen from the list first.");
            }
        }

        private void EditPreviousDiseaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(previousDiseaseTextBox.Text))
            {
                MessageBox.Show("You must enter a disease.");
                return;
            }
            if (previousDiseasesListBox.SelectedItem != null)
            {
                previousDiseasesListBox.Items[previousDiseasesListBox.SelectedIndex] = previousDiseaseTextBox.Text;
            }
            else
            {
                MessageBox.Show("You need to select a disease from the list first.");
            }
        }

        private void EditAllergenButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(allergenTextBox.Text))
            {
                MessageBox.Show("You must enter an allergen.");
                return;
            }
            if (allergensListBox.SelectedItem != null)
            {
                allergensListBox.Items[allergensListBox.SelectedIndex] = allergenTextBox.Text;
            }
            else
            {
                MessageBox.Show("You need to select an allergen from the list first.");
            }
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            idTextBox.Text = (UserService.maxID + 1).ToString();
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

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(firstNameTextBox.Text))
            {
                MessageBox.Show("You must enter a first name.");
                return;
            }
            if (string.IsNullOrWhiteSpace(usernameTextBox.Text))
            {
                MessageBox.Show("You must enter a username.");
                return;
            }
            if (string.IsNullOrWhiteSpace(lastNameTextBox.Text))
            {
                MessageBox.Show("You must enter a last name.");
                return;
            }
            if (string.IsNullOrWhiteSpace(passwordTextBox.Text))
            {
                MessageBox.Show("You must enter a password.");
                return;
            }
            if (string.IsNullOrWhiteSpace(heightTextBox.Text))
            {
                MessageBox.Show("You must enter a height.");
                return;
            }
            if (string.IsNullOrWhiteSpace(weightTextBox.Text))
            {
                MessageBox.Show("You must enter a weight.");
                return;
            }
            if (birthDatePicker.SelectedDate == null)
            {
                MessageBox.Show("You must enter a date of birth.");
                return;
            }
            if (birthDatePicker.SelectedDate > DateTime.Now)
            {
                MessageBox.Show("You cannot enter a date in the future.");
                return;
            }
            //check if username already in use
            foreach (User user in UserRepository.Users)
            {
                if (user.Username == usernameTextBox.Text)
                {
                    MessageBox.Show("Username is already in use. Choose a different one.");
                    return;
                }
            }
            if (!Double.TryParse(heightTextBox.Text, out double height))
            {
                MessageBox.Show("Height must be a number.");
                return;
            }
            if (!Double.TryParse(weightTextBox.Text, out double weight))
            {
                MessageBox.Show("Weight must be a number.");
                return;
            }
            //update max user and health record ID
            HealthRecordService.maxID++;
            UserService.maxID++;
            //create record and patient
            HealthRecord record = new HealthRecord(HealthRecordService.maxID, height, weight, previousDiseasesListBox.Items.Cast<String>().ToList(), allergensListBox.Items.Cast<String>().ToList(), UserService.maxID);
            Patient patient = new Patient(UserService.maxID, usernameTextBox.Text, passwordTextBox.Text, firstNameTextBox.Text, lastNameTextBox.Text, (DateTime)birthDatePicker.SelectedDate, false, Enums.Blocker.None, new List<int>(), HealthRecordService.maxID);
            //add to repositories
            HealthRecordRepository.Records.Add(record);
            UserRepository.Patients.Add(patient);
            UserRepository.Users.Add(patient);
            //save to files
            HealthRecordRepository.Save();
            UserRepository.SavePatients();

            Reset();
            idTextBox.Text = (UserService.maxID + 1).ToString();
            MessageBox.Show("Successfully created the patient and health record.");
        }
    }
}
